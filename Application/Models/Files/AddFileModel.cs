using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Files
{
    public class AddFileModel
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