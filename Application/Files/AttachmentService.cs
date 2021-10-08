using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.Files;
using File = Domain.Files.File;

namespace Application.Files
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IFileCollection _fileCollection;

        public AttachmentService(IFileCollection fileCollection)
        {
            _fileCollection = fileCollection;
        }
        public async Task<FileResponse> UploadFile(FileRequest request, CancellationToken cancellationToken = default)
        {
            var response = new FileResponse();

            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    // Save file to server
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";

                    using var memoryStream = new MemoryStream();
                    
                    // Write file to Stream,
                    await file.CopyToAsync(memoryStream);
                    
                    //Read from mempryStream
                    memoryStream.Position = 0;
                    
                    // Write File to designated 

                    var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(), @"wwwroot/attachments", fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    
                    // Writing memorystream to filestream
                    await memoryStream.CopyToAsync(fileStream);
                    
                    //Add filePath to response

                   response.FileUrls.Add($"{request.BaseUrl}/attachments/{fileName}");
               
                    // await _fileCollection.CreateImage(new File
                    // {
                    //      FileUrl = $"{request.BaseUrl}/attachments/{fileName}"
                    // });
                }
            }
            return response;
        }
    }
}