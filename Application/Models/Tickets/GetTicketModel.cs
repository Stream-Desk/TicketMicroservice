using System;
using System.Collections.Generic;
using System.ComponentModel;
using Domain.Comments;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Application.Models.Tickets
{
    public class GetTicketModel
    {
        public string Id { get; set; }
        public int TicketNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Category  Category { get; set; }
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
        public DateTime ClosureDateTime { get; set; }
        public DateTime ModifiedAt { get; set; }
        public List<Comment> Comments { get; set; } 
        public List<string> FileUrls { get; set; }
    }
}