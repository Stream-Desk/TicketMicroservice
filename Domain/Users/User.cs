using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Users
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [BsonElement("EmailAddress")]
        public string EmailAddress { get; set; }
        [Required]
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [Required]
        [BsonElement("LastName")]
        public string LastName { get; set; }
        // public List<Ticket> TicketList { get; set; } = new List<Ticket>();
    }
}