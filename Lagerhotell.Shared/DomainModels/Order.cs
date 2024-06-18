namespace LagerhotellAPI.Models.DomainModels;

public record Order(string OrderId, string UserId, string StorageUnitId, OrderPeriod OrderPeriod, OrderStatus Status, OrderInsurance Insurance, string? CustomInstructions = null);
