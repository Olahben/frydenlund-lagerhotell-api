global using LagerhotellAPI.Models.ValueTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class StorageUnit
{
    public StorageUnit(string storageUnitId, Dimensions dimensions, bool temperated, string lockCode, string name, bool occupied, string userId, Coordinate coordinate, Money pricePerMonth)
    {
        StorageUnitId = storageUnitId;
        Dimensions = dimensions;
        Temperated = temperated;
        LockCode = lockCode;
        Name = name;
        Occupied = occupied;
        UserId = userId;
        Coordinate = coordinate;
        PricePerMonth = pricePerMonth;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? StorageUnitId { get; set; }
    public Dimensions Dimensions { get; set; }

    public bool Temperated { get; set; }
    public string? LockCode { get; set; }

    public string Name { get; set; }

    public bool Occupied { get; set; } = false;

    public string? UserId { get; set; }

    public Coordinate? Coordinate { get; set; }

    public Money PricePerMonth { get; set; }
}
