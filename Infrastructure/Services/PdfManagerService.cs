using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Infrastructure.Services
{
    public class PdfManagerService : IPdfManagerService
    {
        public PdfManagerService()
        {

        }

        public ICollection<IFormFile> SplitPdf(string originalFileName, IFormFile pdfFile)
        {
            var splittedFiles = new List<IFormFile>();

            using (var originalStream = pdfFile.OpenReadStream())
            {
                using (var originalPdf = PdfReader.Open(originalStream, PdfDocumentOpenMode.Import))
                {
                    for (int pageIndex = 0; pageIndex < originalPdf.PageCount; pageIndex++)
                    {
                        var splittedPdf = new PdfDocument();
                        splittedPdf.AddPage(originalPdf.Pages[pageIndex]);

                        byte[] outputBytes;
                        using (var outputStream = new MemoryStream())
                        {
                            splittedPdf.Save(outputStream);
                            outputBytes = outputStream.ToArray(); 
                        }

                        var outputFormFile = new FormFile(new MemoryStream(outputBytes), 0, outputBytes.Length, "file", $"{originalFileName}_{pageIndex + 1}.pdf")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/pdf"
                        };

                        splittedFiles.Add(outputFormFile);
                    }
                }
            }

            return splittedFiles;
        }
    }
}
