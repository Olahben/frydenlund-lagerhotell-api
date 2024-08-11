namespace LagerhotellAPI.Models.DomainModels;

public record Order(string OrderId, string UserId, string StorageUnitId, OrderPeriod OrderPeriod, OrderStatus Status = OrderStatus.NotCreated, OrderInsurance Insurance = OrderInsurance.FiftyThousand, string? CustomInstructions = null);
