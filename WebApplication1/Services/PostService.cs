using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Post;
using WebApplication1.Entities;
using WebApplication1.Extensions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
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
         * Map data to Response | PostResponseDTO
         */
        private static PostResponseDto MapToResponseDto(Post post, IConfiguration config)
        {
            return new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                IsPublished = post.IsPublished,
                ImageRelativePath = post.ImageRelativePath,
                ImageFullUrl = post.ImageRelativePath.ToFullUrl(config)
            };
        }

        /*
         * Index - Get All Data
         */
        public async Task<IEnumerable<PostResponseDto>> Index()
        {
            var posts = await _context.Posts.ToListAsync();
            //Map Final Data
            return posts.Select(p => MapToResponseDto(p, _config));
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
                return MapToResponseDto(post, _config);
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
         * Find - Get a single Item by ID
         */
        public async Task<PostResponseDto?> Find(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return null;

            //Map DTO
            return MapToResponseDto(post, _config);
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

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                //Delete Old Image
                if (newRelativePath != null && !string.IsNullOrEmpty(oldRelativePath))
                {
                    await _storageService.DeleteFileAsync(oldRelativePath, _containerName);
                }

                return MapToResponseDto(post, _config);
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