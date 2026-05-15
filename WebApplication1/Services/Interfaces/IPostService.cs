using WebApplication1.DTOs.Post;

namespace WebApplication1.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseDto>> Index();
        Task<PostResponseDto?> Find(int id);
        Task<PostResponseDto> Create(PostCreateDto postDto);
        Task<bool> Delete(int id);
    }
}
