using WebApplication1.DTOs.Common;
using WebApplication1.DTOs.Post;

namespace WebApplication1.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagedResponse<PostResponseDto>> Index(int? pageNumber = null, int? pageSize = null, string orderField = null, string orderWay = null);
        Task<PostResponseDto?> Find(int id);
        Task<PostResponseDto> Create(PostCreateDto postDto);
        Task<PostResponseDto?> Update(int id, PostUpdateDto postDto);
        Task<bool> Delete(int id);
    }
}
