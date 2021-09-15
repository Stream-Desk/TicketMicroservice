using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Tickets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Users
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("EmailAddress")]
        public string EmailAddress { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        
        public List<Ticket> TicketList { get; set; } = new List<Ticket>();
    }
}