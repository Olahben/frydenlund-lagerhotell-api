using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required DateTime OrderDate { get; set; }

        public DateTime? EndDate { get; set; }
        public User User { get; set; }
        public StorageUnit StorageUnit { get; set; }
        public OrderStatus Status { get; set; }

        public string? CustomInstructions { get; set; }

    }
}
