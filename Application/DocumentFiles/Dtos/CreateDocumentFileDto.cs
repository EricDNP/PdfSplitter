using Microsoft.AspNetCore.Http;

namespace Application.DocumentFiles.Dtos
{
    public class CreateDocumentFileDto
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
