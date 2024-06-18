global using LagerhotellAPI.Models.DomainModels;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;
public record OrderDocument(string orderId, string userId, OrderPeriod period, string storageUnitId, OrderStatus status, OrderInsurance insurance, string? customInstructions = null)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
