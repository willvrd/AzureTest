using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Modules.Posts.DTOs.Common;
using WebApplication1.Modules.Posts.DTOs.Post;
using WebApplication1.Modules.Posts.Services.Interfaces;

namespace WebApplication1.Modules.Posts.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/blog/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /*
        * Index | Pagination (Optional)
        */
        [HttpGet]
        public async Task<ActionResult<PagedResponse<PostResponseDto>>> Index(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string orderField = "id",
        [FromQuery] string orderWay = "desc")
        {
            // Sanitize pagination if present
            if (pageNumber.HasValue) pageNumber = pageNumber < 1 ? 1 : pageNumber;
            if (pageSize.HasValue) pageSize = pageSize > 50 ? 50 : pageSize;

            var pagedData = await _postService.Index(pageNumber, pageSize, orderField, orderWay);

            return Ok(pagedData);
        }

        

        /*
         * Find by dynamic criteria
         */
        [HttpGet("{value}")]          
        public async Task<ActionResult<ResponseWrapper<PostResponseDto>>> Find(string value, [FromQuery] string field = "id")
        {
           
            //Validation not int
            if (field == "id" && !int.TryParse(value, out _))
            {
                field = "slug";
            }

            var post = await _postService.FindByCriteria(value, field);

            if (post == null)
            {
                return NotFound(new { message = $"Item with {field} '{value}' was not found." });
            }

            return Ok(new ResponseWrapper<PostResponseDto>(post));
        }

        /*
        * Create
        */
        [HttpPost]
        public async Task<ActionResult<ResponseWrapper<PostResponseDto>>> Create([FromForm] PostCreateDto postDto)
        {
            var response = await _postService.Create(postDto);

            // Return with Data wrapper
            return CreatedAtAction(nameof(Index), new { id = response.Id }, new ResponseWrapper<PostResponseDto>(response));
        }

        /*
        * Update
        */
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseWrapper<PostResponseDto>>> Update(int id, [FromForm] PostUpdateDto postDto)
        {
            var response = await _postService.Update(id, postDto);

            if (response == null)
            {
                return NotFound(new { message = $"Item with ID {id} not found" });
            }

            // Wrapped in Data object
            return Ok(new ResponseWrapper<PostResponseDto>(response));
        }

        /*
        * Delete
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.Delete(id);

            if (!result)
            {
                return NotFound(new { message = $"Post with ID {id} not found" });
            }

            return NoContent(); //204
        }

    }


}