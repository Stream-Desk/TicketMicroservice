using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Application.Attachments
{
    public class AttachmentRequest
    {
        public List<IFormFile> Files { get; set; }
        public string  BaseUrl { get; set; }
    }
}