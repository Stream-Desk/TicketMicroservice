using System;
using System.Text.Json.Serialization;
using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Tickets
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        
        [BsonElement("Summary")]
        public string Summary { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; } = DateTime.Now;
        public Status Status { get; set; }
        public User User { get; set; }
        public string Attachment { get; set; }        
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