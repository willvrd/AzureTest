namespace WebApplication1.Modules.Posts.DTOs.Post
{
    public class PostCreateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
    }
}
