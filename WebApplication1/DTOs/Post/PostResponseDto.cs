namespace WebApplication1.DTOs.Post
{
    public class PostResponseDto : PostBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageRelativePath { get; set; }
        public string? ImageFullUrl { get; set; }
    }
}
