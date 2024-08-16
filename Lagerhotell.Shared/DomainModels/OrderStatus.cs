namespace LagerhotellAPI.Models.DomainModels;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    Active,
    PaymentDue,
    Deleted,
    NotCreated
}
