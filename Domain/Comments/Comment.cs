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
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; } 
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID {get;set;}
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketId { get; set; }
    }
}