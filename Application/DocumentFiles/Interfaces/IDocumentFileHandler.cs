using Application.DocumentFiles.Dtos;

namespace Application.DocumentFiles.Interfaces
{
    public interface IDocumentFileHandler
    {
        Task<ICollection<GetDocumentFileDto>> UploadDocumentFile(CreateDocumentFileDto dto);
    }
}
