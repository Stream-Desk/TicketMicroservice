using System;
using System.Collections.Generic;
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
                FileId = model.FileId,   
                Name = model.Name,
                FileType = model.FileType,
                Extension = model.Extension,
                CreatedOn = model.CreatedOn,
                FilePath = model.FilePath,
                FileUrl = model.FileUrl
            };

            var search = await _fileCollection.UploadFile(file, cancellationToken);
            
            var result = new DownloadFileModel
            {
                FileId = search.FileId,   
                Name = search.Name,
                FileType = search.FileType,
                Extension  = search.Extension,
                CreatedOn  = search.CreatedOn,
                FilePath  = search.FilePath,
                FileUrl = search.FileUrl
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
           
            var search = await _fileCollection.DownloadFile(imageId, cancellationToken);
            if (search == null)
            {
                return new DownloadFileModel();
            }
            var result = new DownloadFileModel()
            {
                FileId = search.FileId,   
                Name = search.Name,
                FileType = search.FileType,
                Extension  = search.Extension,
                CreatedOn  = search.CreatedOn,
                FilePath  = search.FilePath,
                FileUrl  = search.FileUrl,
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
            _fileCollection.DeleteFileById(model.FileId);
             return null;
        }

        public async Task<List<DownloadFileModel>> ListImages(CancellationToken cancellationToken = default)
        {
            var searchResults = await _fileCollection.GetFiles(cancellationToken);
            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<DownloadFileModel>();
            }

            var result = new List<DownloadFileModel>();

            foreach (var searchResult in searchResults)
            {
                var model = new DownloadFileModel
                {
                    FileId = searchResult.FileId,   
                    Name = searchResult.Name,
                    FileType = searchResult.FileType,
                    Extension  = searchResult.Extension,
                    CreatedOn  = searchResult.CreatedOn,
                    FilePath  = searchResult.FilePath,
                    FileUrl = searchResult.FileUrl
                };
                result.Add(model);
            }
            return result;
        }
    }
}