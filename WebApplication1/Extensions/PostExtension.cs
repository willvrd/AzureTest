namespace WebApplication1.Extensions
{
    public static class PostExtension
    {
        public static string? ToFullUrl(this string? relativePath, IConfiguration config)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            var baseUrl = config["BlobContainers:BlobBaseUrl"]?.TrimEnd('/');
            var container = config["BlobContainers:ContainerName"]?.TrimStart('/').TrimEnd('/');

            return $"{baseUrl}/{container}/{relativePath.TrimStart('/')}";
        }
    }
}
