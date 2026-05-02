using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class PostDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contenido es obligatorio.")]
        public string Content { get; set; } = string.Empty;
    }
}