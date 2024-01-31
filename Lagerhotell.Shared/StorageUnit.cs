using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models
{
    public class StorageUnit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required double Size { get; set; }

        public required bool Temperated { get; set; }
    }
}
