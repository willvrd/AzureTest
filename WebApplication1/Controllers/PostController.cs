using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Entities;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStorageService _blobStorageService;
        private readonly IConfiguration _configuration;

        public PostsController(ApplicationDbContext context, IStorageService blobStorageService, IConfiguration configuration)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
        }

        /*
         * Index
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Index()
        {
            return await _context.Posts.ToListAsync();
        }

        /*
         * Create
         */
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromForm] PostDto postDto)
        {
            string? imageUrl = null;

            if (postDto.Image != null && postDto.Image.Length > 0)
            {
               
                string containerName = _configuration["BlobContainers:PostImages"] ?? "images";

                imageUrl = await _blobStorageService.UploadFileAsync(postDto.Image, containerName, "posts");
            }

            var post = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                IsPublished = false
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new { id = post.Id }, post);
        }
    }
}