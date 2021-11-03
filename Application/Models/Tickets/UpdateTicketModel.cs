using System.Collections.Generic;
using System.ComponentModel;
using Domain.Comments;
using Domain.Tickets;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public Category Category { get; set; }
        public Priority Priority  { get; set; }
        public Status Status { get; set; }
        public bool IsModified { get; set; }
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool Closed { get; set; }
        public string ClosureDateTime { get; set; }
        public List<string> FileUrls { get; set; } 
        public List<Comment> Comments { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}