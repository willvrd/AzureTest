namespace WebApplication1.Modules.Blog.Posts.DTOs.Post
{
    public class PostResponseDto : PostBaseDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageRelativePath { get; set; }
        public string? ImageFullUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
