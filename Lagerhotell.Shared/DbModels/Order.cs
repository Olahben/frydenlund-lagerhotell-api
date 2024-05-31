global using LagerhotellAPI.Models.DomainModels;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class Order
{
    public Order(string orderId, string userId, OrderPeriod period, string storageUnitId, OrderStatus status, string? customInstructions = null)
    {
        OrderId = orderId;
        UserId = userId;
        OrderPeriod = period;
        StorageUnitId = storageUnitId;
        Status = status;
        CustomInstructions = customInstructions;
    }

    public Order() { }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Should be created upon registration of order
    public string? OrderId { get; set; }
    public OrderPeriod OrderPeriod { get; set; }
    public string UserId { get; set; }
    public string StorageUnitId { get; set; }
    public OrderStatus Status { get; set; }
    public OrderInsurance Insurance { get; set; } = new OrderInsurance();

    public string? CustomInstructions { get; set; }

}
