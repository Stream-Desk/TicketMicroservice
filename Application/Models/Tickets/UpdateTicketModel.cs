using System;
using System.ComponentModel;
using Domain.Tickets;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsModified { get; set; } = false;
        public DateTime? ModifiedAt { get; set; }
    }
}