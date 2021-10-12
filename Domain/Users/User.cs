using System.Collections.Generic;
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
        [BsonElement("UserName")]
        public string UserName { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("Password")]

        public string Password { get; set; }
        public List<Ticket> TicketList { get; set; } = new List<Ticket>();
    }
}