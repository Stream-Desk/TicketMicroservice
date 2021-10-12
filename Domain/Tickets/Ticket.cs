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
        public int TicketNumber { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public string Name { get; set; }
        public object Value { get; private set; }
        public bool IsDeleted { get; set; }
        public bool IsModified { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Closed { get; set; }
        public DateTime ClosureDateTime { get; set; }
        public int ticketNumber { get; set; }
        public List<string> FileUrls { get; set; } = new List<string>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string FileUrl { get; set; }
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

public enum Category
{
    Bug = 1,
    FreezingScreen = 2,
    Uploads = 3,
    Login = 4,
    Other = 5
}


        