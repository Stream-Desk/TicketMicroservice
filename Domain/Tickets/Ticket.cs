using System;
using System.ComponentModel.DataAnnotations;
using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Tickets
{
    public class Ticket : ISoftDelete 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        [Required]
        public string Id { get; set; }  
        [Required]
        [BsonElement("Summary")]
        public string Summary { get; set; }
        [Required]
        [BsonElement("Category")]
        public string Category { get; set; }
        [Required]
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