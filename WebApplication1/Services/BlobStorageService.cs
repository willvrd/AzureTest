using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WebApplication1.Services.Interfaces;

public class BlobStorageService : IStorageService
{
    private readonly string _connectionString;

    public BlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName, string folderName = "")
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Si folderName tiene valor, creamos la ruta virtual "folder/archivo.ext"
        // Si no, solo usamos el nombre del archivo
        var fileName = string.IsNullOrEmpty(folderName)
            ? $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"
            : $"{folderName}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var blobClient = containerClient.GetBlobClient(fileName);

        using (var stream = file.OpenReadStream())
        {
            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
            };
            await blobClient.UploadAsync(stream, options);
        }

        return blobClient.Uri.ToString();
    }
}