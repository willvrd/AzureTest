using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * GET POSTS
         */

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Index()
        {
            return await _context.Posts.ToListAsync();
        }

        /*
         * CREATE
         */
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDto postDto)
        {
            var post = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                CreatedAt = DateTime.UtcNow,
                IsPublished = false
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new { id = post.Id }, post);
        }
    }
}