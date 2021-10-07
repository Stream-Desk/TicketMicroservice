using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Description { get; set; }
        //public int TicketNumber { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public Priority Priority  { get; set; }
        public Status Status { get; set; }
        
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        
        public bool IsModified { get; set; }
        
        public DateTime? ModifiedAt { get; set; }
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool Closed { get; set; }
        public DateTime ClosureDateTime { get; set; }
        public string FileUrl { get; set; }
    }
}