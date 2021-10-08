using System.Threading;
using System.Threading.Tasks;

namespace Application.Files
{
    public interface IAttachmentService
    {
        Task<FileResponse> UploadFile(FileRequest request, CancellationToken cancellationToken = default);
    }
}