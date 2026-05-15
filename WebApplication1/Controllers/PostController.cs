using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Post;
using WebApplication1.Entities;
using WebApplication1.Extensions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /*
         * Index
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> Index()
        {
            var posts = await _postService.Index();
            return Ok(posts);
        }

        /*
        * Create
        */
        [HttpPost]
        public async Task<ActionResult<PostResponseDto>> Create([FromForm] PostCreateDto postDto)
        {
            var response = await _postService.Create(postDto);
            return CreatedAtAction(nameof(Index), new { id = response.Id }, response);
        }

    }


}