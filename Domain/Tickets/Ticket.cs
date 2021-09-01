using System;
using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Tickets
{
    public class Ticket : IIsDeleted 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }        
        [BsonElement("Summary")]
        public string Summary { get; set; }
        [BsonElement("Category")]
        public string Category { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; } 
        public Status Status { get; set; }
        public User User { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }

    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum Status
    {
        Open = 1,
        Pending = 2,
        Resolved = 3,
        Closed = 4
    }
}