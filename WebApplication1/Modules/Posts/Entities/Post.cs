using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Modules.Posts.Entities
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        //[Required]
        [Required(ErrorMessage = "El content es obligatorio.")]
        public string Content { get; set; } = string.Empty;

        
        [MaxLength(250)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public int SortOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = false;

        public string? ImageRelativePath { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
