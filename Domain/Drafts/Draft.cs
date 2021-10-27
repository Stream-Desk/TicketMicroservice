using System;
using System.Collections.Generic;
using Domain.Files;
using Domain.Tickets;
using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Drafts
{
    public class Draft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Priority Priority { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public List<File> Attachments { get; set; } = new List<File>();


    }

}


