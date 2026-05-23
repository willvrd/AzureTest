// -----------------------------------------------------------------------------
// Author:      William Verde
// Date:        2026
// License:     MIT
// Repository:  https://github.com/willvrd/AzureTest
// -----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modules.Media.Services.Interfaces;
using WebApplication1.Modules.Posts.DTOs.Common;
using WebApplication1.Modules.Posts.DTOs.Post;
using WebApplication1.Modules.Posts.Entities;
using WebApplication1.Modules.Posts.Extensions;
using WebApplication1.Modules.Posts.Services.Interfaces;

namespace WebApplication1.Modules.Posts.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostService> _logger; //Logger with PostService
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly string _containerName;

        public PostService(ApplicationDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;

            _containerName = _config["BlobContainers:ContainerName"] ?? "images";
        }


        /*
         * Index - Pagination (Optional)
         */
        public async Task<PagedResponse<PostResponseDto>> Index(
            int? pageNumber = null,
            int? pageSize = null,
            string orderField = "id",
            string orderWay = "desc")
        {
            var query = _context.Posts.AsQueryable();

            // Normalize inputs to avoid casing issues
            orderField = orderField?.ToLower() ?? "id";
            orderWay = orderWay?.ToLower() ?? "desc";

            bool isDescending = orderWay == "desc";

            //Prevent SQL Injection
            query = orderField switch
            {
                "title" => isDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "createdAt" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                "updatedAt" => isDescending ? query.OrderByDescending(p => p.UpdatedAt) : query.OrderBy(p => p.UpdatedAt),
                _ => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id) // Default fallback (Id)
            };

            //Get total count (after sorting setup, but before execution)
            var totalRecords = await query.CountAsync();

            List<Post> posts;

            //Check if pagination is requested
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                posts = await query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();
            }
            else
            {
                // Return all data (already sorted) if no pagination parameters are provided
                posts = await query.ToListAsync();

                // Adjust pageNumber and pageSize for the response metadata
                pageNumber = 1;
                pageSize = totalRecords == 0 ? 1 : totalRecords; // Avoid 0 pageSize
            }

            var postsDto = posts.Select(p => p.ToResponseDto(_config));

            // Final response
            return new PagedResponse<PostResponseDto>(postsDto, pageNumber.Value, pageSize.Value, totalRecords);
        }

        /*
        * Find - Get a single Item by ID
        */
        public async Task<PostResponseDto?> Find(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return null;

            //Map DTO
            return post?.ToResponseDto(_config);
        }

        /*
        * Create an Item
        */
        public async Task<PostResponseDto> Create(PostCreateDto postDto)
        {
            string? relativePath = null;

            if (postDto.Image != null && postDto.Image.Length > 0)
            {
                relativePath = await _storageService.UploadFileAsync(postDto.Image, _containerName, "posts");
            }

            try
            {
                var post = new Post
                {
                    Title = postDto.Title,
                    Content = postDto.Content,
                    ImageRelativePath = relativePath,
                    CreatedAt = DateTime.UtcNow,
                    IsPublished = false
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                //Map Final Data
                return post.ToResponseDto(_config);
            }
            catch (Exception ex)
            {
                // Delete Image if some fails
                if (!string.IsNullOrEmpty(relativePath))
                {
                    await _storageService.DeleteFileAsync(relativePath, _containerName);
                }

                // Important: Re-lanzamos el error para que el GlobalExceptionHandler lo capture
                throw;
            }
        }


        /*
        * Update an Item
        */
        public async Task<PostResponseDto?> Update(int id, PostUpdateDto postDto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return null;

            string? newRelativePath = null;
            string? oldRelativePath = post.ImageRelativePath; //Save Old

            try
            {
                //New Image
                if (postDto.Image != null && postDto.Image.Length > 0)
                {
                    newRelativePath = await _storageService.UploadFileAsync(postDto.Image, _containerName, "posts");
                    post.ImageRelativePath = newRelativePath;
                }

                //Update Attributes
                post.Title = postDto.Title;
                post.Content = postDto.Content;
                post.UpdatedAt = DateTime.UtcNow;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                //Delete Old Image
                if (newRelativePath != null && !string.IsNullOrEmpty(oldRelativePath))
                {
                    await _storageService.DeleteFileAsync(oldRelativePath, _containerName);
                }

                return post.ToResponseDto(_config);
            }
            catch (Exception ex)
            {
                //ROLLBACK: Si la BD falla pero ya habíamos subido la imagen nueva, la borramos
                if (!string.IsNullOrEmpty(newRelativePath))
                {
                    await _storageService.DeleteFileAsync(newRelativePath, _containerName);
                }

                _logger.LogError(ex, "Error updating item {Id}", id);
                throw;
            }
        }

        /*
        * Delete a item and its Image
        */
        public async Task<bool> Delete(int id)
        {
            //Search Post
            var post = await _context.Posts.FindAsync(id);

            if (post == null) return false;

            var imagePath = post.ImageRelativePath;

            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                //Delete File
                if (!string.IsNullOrEmpty(imagePath))
                {
                    await _storageService.DeleteFileAsync(imagePath, _containerName);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting item {Id}", id);
                throw; // GlobalExceptionHandler
            }
        }

    }
}