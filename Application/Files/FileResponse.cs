using System;

namespace Application.Files
{
    public class FileResponse
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FilePath { get; set; }
        public string FileUrl  { get; set; } 
    }
}