using System.Collections.Generic;

namespace Application.Attachments
{
    public class AttachmentResponse
    {
        public List<string> FileUrls { get; set; } = new List<string>();
    }
}