using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WebApplication1.Services.Interfaces;

public class BlobStorageService : IStorageService
{
    private readonly string _connectionString;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
        _blobServiceClient = new BlobServiceClient(_connectionString);
    }

    /*
     * Create File
     */
    public async Task<string> UploadFileAsync(IFormFile file, string containerName, string folderName = "")
    {
        //Get Client
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

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

        //return blobClient.Uri.ToString(); //ruta completa
        return fileName;
    }

    /*
     * Delete File
     */
    public async Task DeleteFileAsync(string relativePath, string containerName)
    {
        if (string.IsNullOrEmpty(relativePath)) return;

        //Get Client
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        var blobClient = containerClient.GetBlobClient(relativePath);

        await blobClient.DeleteIfExistsAsync();
    }
}