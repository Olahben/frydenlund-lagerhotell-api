namespace LagerhotellAPI.Models.DomainModels;

public record Order(string orderId, string userId, string storageUnitId, OrderPeriod orderPeriod, OrderStatus status, OrderInsurance insurance, string? customInstructions = null);
