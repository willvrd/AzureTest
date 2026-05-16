namespace WebApplication1.DTOs.Post
{
    public class PostUpdateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
    }
}
