namespace WebApplication1.Modules.Posts.DTOs.Post
{
    public class PostUpdateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
    }
}
