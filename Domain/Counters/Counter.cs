using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Counters
{
    class Counter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TicketNumber { get; set; }
        public int Value { get; set; }
    }
}
