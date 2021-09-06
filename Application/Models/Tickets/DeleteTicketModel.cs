using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class DeleteTicketModel
    {
        public string Id { get; set; }
        [BsonDefaultValue(false)]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}