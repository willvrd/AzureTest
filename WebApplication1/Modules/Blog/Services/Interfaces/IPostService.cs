using WebApplication1.Modules.Blog.Posts.DTOs.Common;
using WebApplication1.Modules.Blog.Posts.DTOs.Post;

namespace WebApplication1.Modules.Blog.Posts.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagedResponse<PostResponseDto>> Index(int? pageNumber = null, int? pageSize = null, string orderField = null, string orderWay = null);
        Task<PostResponseDto> FindByCriteria(string value, string field);
        Task<PostResponseDto> Create(PostCreateDto postDto);
        Task<PostResponseDto?> Update(int id, PostUpdateDto postDto);
        Task<bool> Delete(int id);
        
    }
}
