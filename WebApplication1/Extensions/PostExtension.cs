using WebApplication1.DTOs.Post;
using WebApplication1.Entities;

namespace WebApplication1.Extensions
{
    public static class PostExtension
    {

        /*
         * Response DTO
         */
        public static PostResponseDto ToResponseDto(this Post post, IConfiguration config)
        {
            return new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                IsPublished = post.IsPublished,
                ImageRelativePath = post.ImageRelativePath,
                ImageFullUrl = post.ImageRelativePath.ToFullUrl(config) //Att ersonalizado
            };
        }

        /*
         * CreateF Full Url | Image |Custom method
         */
        public static string? ToFullUrl(this string? relativePath, IConfiguration config)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            var baseUrl = config["BlobContainers:BlobBaseUrl"]?.TrimEnd('/');
            var container = config["BlobContainers:ContainerName"]?.TrimStart('/').TrimEnd('/');

            return $"{baseUrl}/{container}/{relativePath.TrimStart('/')}";
        }

    }
}