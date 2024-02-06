namespace LagerhotellAPI.Models.DomainModels;

public class Order
{
    public Order(string orderId, OrderPeriod orderPeriod, string userId, string storageUnitId, OrderStatus status, string? customInstructions = null)
    {
        Id = orderId;
        OrderPeriod = orderPeriod;
        UserId = userId;
        StorageUnitId = storageUnitId;
        Status = status;
        CustomInstructions = customInstructions;
    }
    public string Id { get; set; }
    public OrderPeriod OrderPeriod { get; set; }
    public string UserId { get; set; }
    public string StorageUnitId { get; set; }
    public OrderStatus Status { get; set; }

    public string? CustomInstructions { get; set; }

}
