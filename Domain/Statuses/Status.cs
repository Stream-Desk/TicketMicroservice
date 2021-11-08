using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Statuses
{
    public class Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public State State { get; set; }
        public string TicketId { get; set; }
    }

    public enum State
    {
        New = 1,
        Open = 2,
        InProgress = 3,
        Pending = 4,
        Resolved = 5,
        Closed = 6,
    }
}