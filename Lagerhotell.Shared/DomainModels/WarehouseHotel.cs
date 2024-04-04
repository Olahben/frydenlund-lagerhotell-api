namespace LagerhotellAPI.Models.DomainModels;

public class WarehouseHotel
{
    public string? WarehouseHotelId { get; set; }
    public Coordinate Coordinate { get; set; }
    public Address Address { get; set; }
    public string Name { get; set; }
    public OpeningHours OpeningHours { get; set; }
    public string PhoneNumber { get; set; }
    public string DescriptionParaOne { get; set; }
    public string? DescriptionParaTwo { get; set; }
    public string? DescriptionParaThree { get; set; }
    public string? DescriptionParaFour { get; set; }
    public string? DescriptionParaFive { get; set; }
    public bool ContainsTemperatedStorageUnits { get; set; }
    public bool IsActive { get; set; }
    public StorageUnitSizes StorageUnitsSizes { get; set; }
    public string LocationName { get; set; }

    public byte[] ImageData { get; set; }

    public WarehouseHotel(Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string descriptionParaOne, string descriptionParaTwo, string descriptionParaThree, string descriptionParaFour, string descriptionParaFive, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName, byte[] imageData)
    {
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DescriptionParaOne = descriptionParaOne;
        DescriptionParaTwo = descriptionParaTwo;
        DescriptionParaThree = descriptionParaThree;
        DescriptionParaFour = descriptionParaFour;
        DescriptionParaFive = descriptionParaFive;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
        ImageData = imageData;
    }
    public WarehouseHotel(string warehouseHotelId, Coordinate coordinate, Address address, string name, OpeningHours openingHours, string phoneNumber, string descriptionParaOne, string descriptionParaTwo, string descriptionParaThree, string descriptionParaFour, string descriptionParaFive, bool containsTemperatedStorageUnits, bool isActive, StorageUnitSizes storageUnitSizes, string locationName, byte[] imageData)
    {
        WarehouseHotelId = warehouseHotelId;
        Coordinate = coordinate;
        Address = address;
        Name = name;
        OpeningHours = openingHours;
        PhoneNumber = phoneNumber;
        DescriptionParaOne = descriptionParaOne;
        DescriptionParaTwo = descriptionParaTwo;
        DescriptionParaThree = descriptionParaThree;
        DescriptionParaFour = descriptionParaFour;
        DescriptionParaFive = descriptionParaFive;
        ContainsTemperatedStorageUnits = containsTemperatedStorageUnits;
        IsActive = isActive;
        StorageUnitsSizes = storageUnitSizes;
        LocationName = locationName;
        ImageData = imageData;
    }

    public WarehouseHotel()
    {
        // Parameterless constructor
    }
}
