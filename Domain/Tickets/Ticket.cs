using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Comments;
using Domain.Files;
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
        public string Summary { get; set; }
        public string TicketNumber { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        
        public object Value { get; private set; }
        public bool IsDeleted { get; set; }
        public bool IsModified { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Closed { get; set; }
        public DateTime ClosureDateTime { get; set; }
        public int ticketNumber { get; set; }
        public List<File> Attachments { get; set; } = new List<File>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
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
        Resolved = 3
    }


        