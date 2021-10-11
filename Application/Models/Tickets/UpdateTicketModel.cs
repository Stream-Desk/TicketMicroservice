using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Description { get; set; }
        //public int TicketNumber { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public Category Category { get; set; }
        public Priority Priority  { get; set; }
        public Status Status { get; set; }
    
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        
        public bool IsModified { get; set; }
        
        public DateTime? ModifiedAt { get; set; }
        [DefaultValue(false)]
        public bool Closed { get; set; }
        public DateTime ClosureDateTime { get; set; }
       
    }
}