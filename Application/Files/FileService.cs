using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Files;
using Domain.Files;
using File = Domain.Files.File;

namespace Application.Files
{
    public class FileService : IFileService
    {
        private readonly IFileCollection _fileCollection;

        public FileService(IFileCollection fileCollection)
        {
            _fileCollection = fileCollection;
        }
        public async Task<DownloadFileModel> UploadFile(AddFileModel model, CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                throw new Exception("Image details empty");
            }
            
            // Map model to domain Entity
            var file = new File()
            {
                 Id = model.Id,   
                 Name = model.Name,
                FileType = model.FileType,
                Extension = model.Extension,
                CreatedOn = model.CreatedOn,
                FilePath = model.FilePath,
            };

            var search = await _fileCollection.CreateImage(file, cancellationToken);
            var result = new DownloadFileModel
            {
                Id = search.Id,   
                Name = search.Name,
                FileType = search.FileType,
                Extension  = search.Extension,
                CreatedOn  = search.CreatedOn,
                FilePath  = search.FilePath,
            };
            return result;
        }

        public  async Task<DownloadFileModel> DownloadImage(string imageId, CancellationToken cancellationToken = default)
        {
            // validate
            if (string.IsNullOrWhiteSpace(imageId))
            {
                throw new Exception("Ticket not Found");
            }
           
            var search = await _fileCollection.DownloadImage(imageId, cancellationToken);
            if (search == null)
            {
                return new DownloadFileModel();
            }
            var result = new DownloadFileModel()
            {
                Id = search.Id,   
                Name = search.Name,
                FileType = search.FileType,
                Extension  = search.Extension,
                CreatedOn  = search.CreatedOn,
                FilePath  = search.FilePath,
            };
            return result;
        }
        
        
        public  Task<DeleteFileModel> DeleteFile(DeleteFileModel model, CancellationToken cancellationToken = default)
        {
            // validation
            if (model == null)
            {
                throw new Exception("Ticket Id not found");
            }
            _fileCollection.DeleteImageById(model.Id);
             return null;
        }
    }
}