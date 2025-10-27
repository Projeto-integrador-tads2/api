using Microsoft.AspNetCore.Http;

namespace Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder, string? fileName = null);
        Task<bool> DeleteFileAsync(string fileKey);
        Task<Stream> DownloadFileAsync(string fileKey);
        string GetPublicUrl(string fileKey);
    }
}
