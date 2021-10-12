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
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("FileType")]
        public string FileType { get; set; }
        [BsonElement("Extension")]
        public string Extension { get; set; }
        [BsonElement("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [BsonElement("FilePath")]
        public string FilePath { get; set; }
        [BsonElement("FileUrl")]
        public string FileUrl  { get; set; }

    }
}