namespace LagerhotellAPI.Models.DomainModels;

// Used to display orders and storage units together when both of their data is needed
public record OrderAndStorageUnit(List<Order> Orders, List<StorageUnit> StorageUnits);
