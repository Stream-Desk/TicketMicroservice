using System;
using System.Text.Json.Serialization;
using Domain.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Drafts
{
    public class Draft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Summary")]

        public string Summary { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Priority Priority { get; set; }
        public DateTime SubmitDate { get; set; } = DateTime.Now.ToLocalTime();
        public Status Status { get; set; }
        public User User { get; set; }
        public string Attachment { get; set; }


    }

}


