using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [BsonElement("Summary")]
        public string Summary { get; set; }
        public string TicketNumber { get; set; }
        [Required]
        [BsonElement("Category")]
        public string Category { get; set; }
        [Required]
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Priority")]
        public Priority Priority { get; set; }
        [BsonElement("SubmitDate")]
        public DateTime SubmitDate { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        public User User { get; set; }

        public object Value { get; private set; }
        public bool IsDeleted { get; set; }
        public bool IsModified { get; set; }

        [BsonElement("ModifiedAt")]
        public DateTime ModifiedAt { get; set; }
        public bool Closed { get; set; }

        public DateTime ClosureDateTime { get; set; }

        public List<File> Files { get; set; } = new List<File>();


    
    public string QueueMessageId { get; set; }

    public string QueuePopReceipt { get; set; }

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
}


        