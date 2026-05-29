namespace WebApplication1.Modules.Posts.DTOs.Post
{
    public class PostCreateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}
