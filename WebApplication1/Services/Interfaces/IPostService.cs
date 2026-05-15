using WebApplication1.DTOs.Post;

namespace WebApplication1.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseDto>> Index();
        Task<PostResponseDto> Create(PostCreateDto postDto);
    }
}
