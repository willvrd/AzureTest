namespace WebApplication1.Modules.Blog.Posts.DTOs.Post
{
    public class PostUpdateDto : PostBaseDto
    {
        public IFormFile? Image { get; set; }
        public int SortOrder { get; set; }
    }
}
