using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Files;

namespace Application.Files
{
    public interface IFileService
    {
        Task<DownloadFileModel> UploadFile(AddFileModel model, CancellationToken cancellationToken = default);
        Task<DownloadFileModel> DownloadImage(string imageId, CancellationToken cancellationToken = default);
        Task<List<DownloadFileModel>> ListImages(CancellationToken cancellationToken = default);
    }
}