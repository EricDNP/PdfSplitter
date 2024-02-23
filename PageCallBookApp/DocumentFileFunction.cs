using Application.DocumentFiles.Dtos;
using Application.DocumentFiles.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PageCallBookApp
{
    public class DocumentFileFunction
    {
        private readonly ILogger _logger;
        private readonly IDocumentFileHandler _documentFileHandler;

        public DocumentFileFunction(ILoggerFactory loggerFactory, IDocumentFileHandler documentFileHandler)
        {
            _logger = loggerFactory.CreateLogger<DocumentFileFunction>();
            _documentFileHandler = documentFileHandler;
        }

        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        [Function("DocumentFile")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("DocumentFileFunction");

            var formData = await req.ReadFormAsync();

            var file = formData.Files.GetFile("file");

            if (file == null || file.Length < 0)
                throw new Exception($"Error uploading file: {file.FileName}");

            string name = formData["name"];

            var dto = new CreateDocumentFileDto()
            {
                Name = name,
                File = file,
            };

            var response = await _documentFileHandler.UploadDocumentFile(dto);

            return new OkObjectResult(response);
        }
    }
}
