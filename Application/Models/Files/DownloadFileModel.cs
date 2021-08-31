using System;

namespace Application.Models.Files
{
    public class DownloadFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FilePath { get; set; }
        public string urlPath { get; set; }
    }
}