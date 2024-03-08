using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services;

public class WarehouseHotelService
{
    private readonly IMongoCollection<LagerhotellAPI.Models.DbModels.WarehouseHotel> _warehouseHotels;
    public WarehouseHotelService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _warehouseHotels = database.GetCollection<LagerhotellAPI.Models.DbModels.WarehouseHotel>("WarehouseHotels");
    }
    public async Task<string> AddWarehouseHotel(WarehouseHotel warehouseHotel)
    {
        string id = Guid.NewGuid().ToString();
        // Should check if there exists one with the same name
        Models.DbModels.WarehouseHotel dbWarehouseHotel = new(id, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.IsActive, warehouseHotel.StorageUnitsSizes);
        await _warehouseHotels.InsertOneAsync(dbWarehouseHotel);
        return id;
    }

    public async Task<(string, string)> DeleteWarehouseHotel(string id)
    {
        WarehouseHotel deletedWarehouseHotel = await GetWarehouseHotelById(id);
        if (deletedWarehouseHotel == null)
        {
            throw new KeyNotFoundException();
        }
        await _warehouseHotels.DeleteOneAsync(hotel => hotel.WarehouseHotelId == id);
        return (deletedWarehouseHotel.WarehouseHotelId, deletedWarehouseHotel.Name);
    }

    public async Task ModifyWarehouseHotel(WarehouseHotel warehouseHotel, string oldName)
    {
        Models.DbModels.WarehouseHotel oldWarehouseHotel = await GetWarehouseHotelByIdDbModel(warehouseHotel.WarehouseHotelId);
        if (oldWarehouseHotel != null)
        {
            LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = new(oldWarehouseHotel.Id, warehouseHotel.WarehouseHotelId, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.IsActive, warehouseHotel.StorageUnitsSizes);
            await _warehouseHotels.ReplaceOneAsync(hotel => hotel.Name == oldName, dbWarehouseHotel);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    public async Task<WarehouseHotel> GetWarehouseHotelById(string id)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.WarehouseHotelId == id).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        WarehouseHotel domainWarehouseHotel = new(dbWarehouseHotel.WarehouseHotelId, dbWarehouseHotel.Coordinate, dbWarehouseHotel.Address, dbWarehouseHotel.Name, dbWarehouseHotel.OpeningHours, dbWarehouseHotel.PhoneNumber, dbWarehouseHotel.DetailedDescription, dbWarehouseHotel.ContainsTemperatedStorageUnits, dbWarehouseHotel.IsActive, dbWarehouseHotel.StorageUnitsSizes);
        return domainWarehouseHotel;
    }

    public async Task<WarehouseHotel> GetWarehouseHotelByName(string name)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.Name == name).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        WarehouseHotel domainWarehouseHotel = new(dbWarehouseHotel.WarehouseHotelId, dbWarehouseHotel.Coordinate, dbWarehouseHotel.Address, dbWarehouseHotel.Name, dbWarehouseHotel.OpeningHours, dbWarehouseHotel.PhoneNumber, dbWarehouseHotel.DetailedDescription, dbWarehouseHotel.ContainsTemperatedStorageUnits, dbWarehouseHotel.IsActive, dbWarehouseHotel.StorageUnitsSizes);
        return domainWarehouseHotel;
    }

    public async Task<Models.DbModels.WarehouseHotel> GetWarehouseHotelByIdDbModel(string id)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.WarehouseHotelId == id).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        return dbWarehouseHotel;
    }

    public async Task<List<WarehouseHotel>> GetAllWarehouseHotels(int? skip = 0, int? take = 0)
    {
        List<Models.DbModels.WarehouseHotel> dbWarehouseHotels = await _warehouseHotels.Find(hotel => true).Skip(skip).Limit(take).ToListAsync();
        List<WarehouseHotel> domainWarehouseHotels = dbWarehouseHotels.ConvertAll(hotel =>
        {
            if (hotel == null || hotel.WarehouseHotelId == null)
            {
                throw new KeyNotFoundException();
            }
            return new WarehouseHotel(hotel.WarehouseHotelId, hotel.Coordinate, hotel.Address, hotel.Name, hotel.OpeningHours, hotel.PhoneNumber, hotel.DetailedDescription, hotel.ContainsTemperatedStorageUnits, hotel.IsActive, hotel.StorageUnitsSizes);
        });
        return domainWarehouseHotels;
    }
}
