namespace WebApplication1.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName, string folderName = "");
    }
}
