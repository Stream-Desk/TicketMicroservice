using System;
using System.IO;
using System.Threading.Tasks;
using Domain.Files;
using Domain.Tickets;
using MongoDB.Bson;
using File = Domain.Files.File;

namespace Application.Attachments
{
    public class AttachmentService : IAttachmentService
    {
         private readonly IFileCollection _fileCollection;
         private readonly ITicketCollection _ticketCollection;


         public AttachmentService(IFileCollection fileCollection, ITicketCollection ticketCollection)
         {
             _fileCollection = fileCollection;
             _ticketCollection = ticketCollection;
         }
        public async Task<AttachmentResponse> UploadAttachmentAsync(AttachmentRequest request, string ticketId)
        {
            var response = new AttachmentResponse();
            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    // Get File Details
                    var fileId = ObjectId.GenerateNewId().ToString();
                    var fileName =  Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_"));
                    var fileExtension = Path.GetExtension(file.FileName);
                    
                    await using var memoryStream = new MemoryStream();

                    // Write to Stream
                    await file.CopyToAsync(memoryStream);

                    // Read from the Start of the Stream
                    memoryStream.Position = 0;

                    // Write to Specific Location
                    var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "Files", $"{fileName}{fileExtension}");

                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    
                    //Write  memoryStream to fileStream
                    await memoryStream.CopyToAsync(fileStream);
                    
                    // Add file Path to response
                    response.FileUrls.Add($"{request.BaseUrl}/api/Files/{fileId}");
                    
                    // Call Ticket By Id

                    var ticket = await _ticketCollection.GetTicketById(ticketId);
                    
                    // Add response URL to the Ticket
                    
                    ticket.FileUrls = response.FileUrls;
                    
                    _ticketCollection.UpdateTicket(ticketId, ticket);
                    
                    await _fileCollection.UploadFile(
                        new File
                        {
                            FileId = fileId,
                            Extension = fileExtension,
                            CreatedOn = DateTime.Now,
                            FilePath = filePath,
                            FileType = file.ContentType,
                            Name = fileName,
                            FileUrl =  $"{request.BaseUrl}/api/Files/{fileId}"
                        });
                }

            }
            return response;
        }
    }
}