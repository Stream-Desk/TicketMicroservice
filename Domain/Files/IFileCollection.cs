using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Files
{
    public interface IFileCollection
    {
        Task<File> DownloadImage(string imageId, CancellationToken cancellationToken = default);
        Task<File> CreateImage(File file, CancellationToken cancellationToken = default);
        void DeleteImageById(string imageId);
    }
}