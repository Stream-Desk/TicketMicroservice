using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Files;
using Domain.Files;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Ocsp;
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
                fileUrl = model.fileUrl
            };

            var search = await _fileCollection.CreateImage(file, cancellationToken);
            
            var result = new DownloadFileModel
            {
                FileId = search.FileId,   
                Name = search.Name,
                FileType = search.FileType,
                Extension  = search.Extension,
                CreatedOn  = search.CreatedOn,
                FilePath  = search.FilePath,
                fileUrl = search.fileUrl
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
                FileId = search.FileId,   
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
            _fileCollection.DeleteImageById(model.FileId);
             return null;
        }
        
    }
}