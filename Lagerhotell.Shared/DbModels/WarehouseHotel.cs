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
    public string DetailedDescription { get; set; }
    public bool ContainsTemperatedStorageUnits { get; set; }
    public bool IsActive { get; set; }
    public StorageUnitSizes StorageUnitsSizes { get; set; }
    public string LocationName { get; set; }
    public byte[] ImageData { get; set; }

    public WarehouseHotel(Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string detailedDescription, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName, byte[] imageData)
    {
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DetailedDescription = detailedDescription;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        StorageUnitsSizes = storageUnitSizes;
        IsActive = isActive;
        LocationName = locationName;
        ImageData = imageData;
    }
    public WarehouseHotel(string warehouseHotelId, Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string detailedDescription, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName, byte[] imageData)
    {
        WarehouseHotelId = warehouseHotelId;
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DetailedDescription = detailedDescription;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
        ImageData = imageData;
    }

    public WarehouseHotel(string id, string warehouseHotelId, Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string detailedDescription, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName)
    {
        Id = id;
        WarehouseHotelId = warehouseHotelId;
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DetailedDescription = detailedDescription;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
    }

    public WarehouseHotel()
    {
        // Parameterless constructor
    }

}
