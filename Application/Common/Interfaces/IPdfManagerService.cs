using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IPdfManagerService
    {
        ICollection<IFormFile> SplitPdf(string originalFileName, IFormFile pdfFile);
    }
}
