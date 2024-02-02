using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models
{
    public class StorageUnit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required double Length { get; set; }

        public required double Width { get; set; }

        public required double Height { get; set; }

        public required bool Temperated { get; set; }

        public required string Area { get; set; }
        public string? LockCode { get; set; }

        public required string Name { get; set; }

        public double AreaSize => Length * Width;

        public double Volume => Length * Width * Height;

        public bool Occupied { get; set; } = false;

        public string? UserId { get; set; }

        public Coordinate? Coordinate { get; set; }

        public required double PricePerMonth { get; set; }
    }
}
