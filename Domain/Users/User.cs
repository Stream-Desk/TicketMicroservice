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
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}