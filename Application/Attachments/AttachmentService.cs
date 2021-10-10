using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.Files;
using MongoDB.Bson;
using File = Domain.Files.File;

namespace Application.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IFileCollection _fileCollection;

        public AttachmentService(IFileCollection fileCollection)
        {
            _fileCollection = fileCollection;
        }
        public async Task<AttachmentResponse> UploadAttachmentAsync(AttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var response = new AttachmentResponse();
            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    // Save file to server
                    var fileId = ObjectId.GenerateNewId().ToString();
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    await using var memoryStream = new MemoryStream();

                    // Write to Stream
                    await file.CopyToAsync(memoryStream);

                    // Read from the Start of the Stream
                    memoryStream.Position = 0;

                    // Write to Specific Location
                    var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(), @"wwwroot/Files", fileName);

                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    
                    //Write  memoryStream to fileStream
                    await memoryStream.CopyToAsync(fileStream);
                    
                    // Add file Path to response
                    response.FileUrls.Add($"{request.BaseUrl}/Files/{fileId}");

                    await _fileCollection.CreateImage(
                        new File
                        {
                            FileId = fileId,
                            Name = fileName,
                            Extension = fileExtension,
                            CreatedOn = DateTime.Now,
                            FileUrl =  $"{request.BaseUrl}/api/Files/{fileId}"
                        });
                }

            }
            return response;
        }
    }
}