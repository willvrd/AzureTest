// -----------------------------------------------------------------------------
// Author:      William Verde
// Date:        2026
// License:     MIT
// Repository:  https://github.com/willvrd/AzureTest
// -----------------------------------------------------------------------------

using WebApplication1.Modules.Posts.DTOs.Post;
using WebApplication1.Modules.Posts.Entities;

namespace WebApplication1.Modules.Posts.Extensions
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
                UpdatedAt = post.UpdatedAt,
                IsPublished = post.IsPublished,
                ImageRelativePath = post.ImageRelativePath,
                ImageFullUrl = post.ImageRelativePath.ToFullUrl(config) //Att personalizado
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