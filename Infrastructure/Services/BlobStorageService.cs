using Application.Common.Interfaces;
using Application.Common.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly ILogger _logger;

        public BlobStorageService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BlobStorageService>();
        }

        private static async Task<BlobContainerClient> CreateBlobContainer(string containerName, string blobStorageConnectionString)
        {
            var container = new BlobContainerClient(blobStorageConnectionString, containerName);

            var containerResponse = await container.CreateIfNotExistsAsync();

            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            if (containerResponse != null && containerResponse.GetRawResponse().IsError)
                throw new Exception($"Error creating container {containerName}");

            return container;
        }

        private static string GetTypeFromContentType(string contentType)
        {
            var types = new Dictionary<string, string>()
            {
                {"application/pdf", ".pdf" },
            };

            var result = types[contentType];

            return result ?? "";
        }

        public async Task<bool> CreateDirectory(string directoryName, string containerName, string blobStorageConnectionString)
        {
            try
            {
                await CreateBlobContainer(containerName, blobStorageConnectionString);

                string blobName = directoryName + "/";

                var blobServiceClient = new BlobClient(blobStorageConnectionString, containerName, blobName);

                using (var stream = new System.IO.MemoryStream())
                {
                    await blobServiceClient.UploadAsync(stream, true);
                } 

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating directory: {directoryName}");
            }
        }
            
        public async Task<BlobResponse> UploadFile(IFormFile file, string directoryName, string containerName, string blobStorageConnectionString)
        {
            _logger.LogInformation($"File: {file.FileName}");

            try
            {
                await CreateBlobContainer(containerName, blobStorageConnectionString);

                var blobServiceClient = new BlobClient(blobStorageConnectionString, containerName, $"{directoryName}/{file.FileName}");

                using (Stream fs = file.OpenReadStream())
                {
                    await blobServiceClient.UploadAsync(fs);
                }

                var blobUrl = blobServiceClient.Uri.AbsoluteUri;

                return new BlobResponse()
                {
                    BlobName = file.FileName,
                    BlobUrl = blobUrl,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {file.FileName}");
            }
        }


    }
}
