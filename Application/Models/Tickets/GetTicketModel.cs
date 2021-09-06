using System;
using System.ComponentModel;
using Domain.Tickets;
using Domain.Users;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class GetTicketModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string  Category { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        // public User User { get; set; }
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } 
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsModified { get; set; }

        public DateTime ModifiedAt { get; set; }
        
    }
}