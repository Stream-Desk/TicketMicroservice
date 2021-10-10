using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        public async Task<AttachmentResponse> UploadAttachmentAsync(AttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var response = new AttachmentResponse();
            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    // Save file to server
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    
                    using var memoryStream = new MemoryStream();

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
                    response.FileUrls.Add($"{request.BaseUrl}/Files/{fileName}");

                }

                return response;
            }
        }
    }
}