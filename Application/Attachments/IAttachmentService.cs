using System.Threading.Tasks;

namespace Application.Attachments
{
    public interface IAttachmentService
    {
        Task<AttachmentResponse> UploadAttachmentAsync(AttachmentRequest request);
    }
}