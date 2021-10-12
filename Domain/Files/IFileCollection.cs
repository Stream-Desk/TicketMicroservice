using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Files
{
    public interface IFileCollection
    {
        Task<File> DownloadFile(string imageId, CancellationToken cancellationToken = default);
        Task<List<File>> GetFiles(CancellationToken cancellationToken = default);
        Task<File> UploadFile(File file, CancellationToken cancellationToken = default);
        void DeleteFileById(string imageId);
    }
}