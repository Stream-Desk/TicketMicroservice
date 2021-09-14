using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class AddTicketModel
    {
        public string Summary { get; set; }
        public string TicketNumber { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Priority Priority  { get; set; }
        
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } 
        
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsModified { get; set; }
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool Closed { get; set; }
        
        public DateTime ModifiedAt { get; set; }
       
        public DateTime ClosureDateTime { get; set; }
    }
}