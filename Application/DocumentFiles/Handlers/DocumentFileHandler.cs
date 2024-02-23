using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DocumentFiles.Dtos;
using Application.DocumentFiles.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.DocumentFiles.Handlers
{
    public class DocumentFileHandler : IDocumentFileHandler
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IPdfManagerService _pdfManagerService;
        private readonly IBlobStorageService _blobSorageService;

        public DocumentFileHandler(ILoggerFactory loggerFactory, IConfiguration configuration, IPdfManagerService pdfManagerService, IBlobStorageService blobStorageService)
        {
            _logger = loggerFactory.CreateLogger<DocumentFileHandler>();

            _configuration = configuration;
            _pdfManagerService = pdfManagerService;
            _blobSorageService = blobStorageService;
        }

        private async Task<GetDocumentFileDto> UploadFile(string directoryName, IFormFile file)
        {
            string blobStorageConnectionString = _configuration.GetValue<string>("BLOB_STORAGE_CONNECTION_STRING");

            BlobResponse blobResponse = await _blobSorageService.UploadFile(file, directoryName, "books", blobStorageConnectionString);

            var documentFile = new GetDocumentFileDto()
            {
                FileName = blobResponse.BlobName,
                OriginalName = file.FileName,
                Url = blobResponse.BlobUrl,
                ContentType = file.ContentType,
                Size = file.Length
            };

            return documentFile;
        }

        public async Task<ICollection<GetDocumentFileDto>> UploadDocumentFile(CreateDocumentFileDto dto)
        {
            _logger.LogInformation("DocumentFileHandler");

            if (dto.File == null)
                throw new Exception($"Error uploading file: {dto.Name}");

            var uploadedFiles = new List<GetDocumentFileDto>();

            var splittedFiles = _pdfManagerService.SplitPdf(dto.Name, dto.File);

            foreach (var splittedFile in splittedFiles)
            {
                var documentFile = await UploadFile(dto.Name, splittedFile);
                uploadedFiles.Add(documentFile);
            }

            return uploadedFiles;
        }
    }
}
