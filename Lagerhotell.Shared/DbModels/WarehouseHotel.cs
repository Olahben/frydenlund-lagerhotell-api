using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class WarehouseHotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? WarehouseHotelId { get; set; }
    public Coordinate Coordinate { get; set; }
    public Address Address { get; set; }
    public string Name { get; set; }
    public OpeningHours OpeningHours { get; set; }
    public string PhoneNumber { get; set; }
    public string? DetailedDescription { get; set; }
    public bool ContainsTemperatedStorageUnits { get; set; }
    public StorageUnitSizes StorageUnitsSizes { get; set; }

    public WarehouseHotel(Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, bool containsTemperatedStorageUnits, StorageUnitSizes storageUnitSizes)
    {
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        StorageUnitsSizes = storageUnitSizes;
    }

}
