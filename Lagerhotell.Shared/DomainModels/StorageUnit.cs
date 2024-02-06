namespace LagerhotellAPI.Models.DomainModels;

public class StorageUnit
{
    public StorageUnit(string storageUnitId, Dimensions dimensions, bool temperated, string lockCode, string name, bool occupied, string userId, Coordinate coordinate, Money pricePerMonth)
    {
        Id = storageUnitId;
        Dimensions = dimensions;
        Temperated = temperated;
        LockCode = lockCode;
        Name = name;
        Occupied = occupied;
        UserId = userId;
        Coordinate = coordinate;
        PricePerMonth = pricePerMonth;
    }
    public string? Id { get; set; }
    public Dimensions Dimensions { get; set; }

    public bool Temperated { get; set; }

    public string? LockCode { get; set; }

    public string Name { get; set; }
    public bool Occupied { get; set; } = false;

    public string? UserId { get; set; }

    public Coordinate? Coordinate { get; set; }

    public Money PricePerMonth { get; set; }
}
