using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Files
{
    public class File
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FilePath { get; set; }
        public string fileUrl  { get; set; }
    }
}