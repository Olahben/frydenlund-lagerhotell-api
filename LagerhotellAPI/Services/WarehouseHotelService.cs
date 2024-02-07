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
    public async Task AddWarehouseHotel(WarehouseHotel warehouseHotel)
    {
        Models.DbModels.WarehouseHotel dbWarehouseHotel = new(warehouseHotel.WarehouseHotelId, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.StorageUnitsSizes);
        await _warehouseHotels.InsertOneAsync(dbWarehouseHotel);
    }

    public async Task DeleteWarehouseHotel(string id)
    {
        await _warehouseHotels.DeleteOneAsync(hotel => hotel.Id == id);
    }

    public async Task ModifyWarehouseHotel(WarehouseHotel warehouseHotel)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = new(warehouseHotel.WarehouseHotelId, warehouseHotel.Coordinate, warehouseHotel.Address, warehouseHotel.Name, warehouseHotel.OpeningHours, warehouseHotel.PhoneNumber, warehouseHotel.DetailedDescription, warehouseHotel.ContainsTemperatedStorageUnits, warehouseHotel.StorageUnitsSizes);
        await _warehouseHotels.ReplaceOneAsync(hotel => hotel.WarehouseHotelId == warehouseHotel.WarehouseHotelId, dbWarehouseHotel);
    }

    public async Task<WarehouseHotel> GetWarehouseHotelById(string id)
    {
        LagerhotellAPI.Models.DbModels.WarehouseHotel dbWarehouseHotel = await _warehouseHotels.Find(hotel => hotel.Id == id).FirstOrDefaultAsync();
        if (dbWarehouseHotel == null || dbWarehouseHotel.WarehouseHotelId == null)
        {
            throw new KeyNotFoundException();
        }
        WarehouseHotel domainWarehouseHotel = new(dbWarehouseHotel.WarehouseHotelId, dbWarehouseHotel.Coordinate, dbWarehouseHotel.Address, dbWarehouseHotel.Name, dbWarehouseHotel.OpeningHours, dbWarehouseHotel.PhoneNumber, dbWarehouseHotel.DetailedDescription, dbWarehouseHotel.ContainsTemperatedStorageUnits, dbWarehouseHotel.StorageUnitsSizes);
        return domainWarehouseHotel;
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
            return new WarehouseHotel(hotel.WarehouseHotelId, hotel.Coordinate, hotel.Address, hotel.Name, hotel.OpeningHours, hotel.PhoneNumber, hotel.DetailedDescription, hotel.ContainsTemperatedStorageUnits, hotel.StorageUnitsSizes);
        });
        return domainWarehouseHotels;
    }
}
