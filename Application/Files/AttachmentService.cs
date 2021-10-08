using System.Threading;
using System.Threading.Tasks;

namespace Application.Files
{
    public class AttachmentService : IAttachmentService
    {
        public Task<FileResponse> UploadFile(FileRequest request, CancellationToken cancellationToken = default)
        {
            
        }
    }
}