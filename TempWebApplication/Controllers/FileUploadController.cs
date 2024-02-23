using Application.DocumentFiles.Dtos;
using Application.DocumentFiles.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TempWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IDocumentFileHandler _documentFileHandler;

        public FileUploadController(IDocumentFileHandler documentFileHandler)
        {
            _documentFileHandler = documentFileHandler;
        }

        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost(Name = "DocumentFile")]
        public async Task<IActionResult> UploadFile([FromForm] CreateDocumentFileDto dto)
        {
            try
            {
                var response = await _documentFileHandler.UploadDocumentFile(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
