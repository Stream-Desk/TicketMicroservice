using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Comments
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Text")]
        public string Text { get; set; }
        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; } 
    }
}