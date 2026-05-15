namespace WebApplication1.DTOs.Post
{
    public class PostCreateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
    }
}
