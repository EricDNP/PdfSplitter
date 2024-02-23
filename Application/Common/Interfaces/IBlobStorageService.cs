using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IBlobStorageService
    {
        Task<bool> CreateDirectory(string directoryName, string containerName, string blobStorageConnectionString);
        Task<BlobResponse> UploadFile(IFormFile file, string directoryName, string containerName, string blobStorageConnectionString);
    }
}
