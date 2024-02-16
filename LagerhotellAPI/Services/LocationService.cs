namespace LagerhotellAPI.Services;

using LagerhotellAPI.Models;
using MongoDB.Driver;
public class LocationService
{
    private readonly IMongoCollection<Models.DbModels.Location> _locations;
    public LocationService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _locations = database.GetCollection<Models.DbModels.Location>("Locations");
    }

    public async Task Add(Location location)
    {
        try
        {
            if (await Get(location.Name) != null)
            {
                throw new InvalidOperationException("Location already exists");
            }
            Models.DbModels.Location dbLocation = new(location.Name, location.IsActive);
            await _locations.InsertOneAsync(dbLocation);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Location already exists, error in LocationService.Add:", ex);
            throw new InvalidOperationException("Could not add a location with the location name" + location.Name);
        }
    }

    public async Task Delete(string name)
    {
        try
        {

            if (await Get(name) != null)
            {
                await _locations.DeleteOneAsync(location => location.Name == name);
            }
            else
            {
                throw new InvalidOperationException("Could not delete the location with the name" + name);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error in LocationService.Delete, could not delete the location with the name" + name, ex);
        }
    }

    public async Task Modify(Location location)
    {
        try
        {
            if (await Get(location.Name) != null)
            {
                Models.DbModels.Location updatedLocation = new(location.Name, location.IsActive);
            }
            else
            {
                throw new InvalidOperationException("Could not find the location with the name" + location.Name);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error in LocationService.Modify", ex);
        }
    }

    public async Task<Location?> Get(string name)
    {
        try
        {
            Models.DbModels.Location dbLocation = await _locations.Find(location => location.Name == location.Name).FirstOrDefaultAsync();
            if (dbLocation == null)
            {
                return null;
            }
            Location domainLocation = new(dbLocation.Name, dbLocation.IsActive);
            return domainLocation;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in LocationService.Get", ex);
            throw new KeyNotFoundException("No location found with the name" + name);
        }
    }

    public async Task<List<Location>>? GetAllActive(bool includeFlagged, int? skip, int? take)
    {
        var filter = Builders<LagerhotellAPI.Models.DbModels.Location>.Filter.Empty;

        if (includeFlagged)
        {
            filter = Builders<LagerhotellAPI.Models.DbModels.Location>.Filter.Eq(Location => Location.IsActive, true);
        }

        List<Models.DbModels.Location> dbLocations = await _locations.Find(filter).Skip(skip).Limit(take).ToListAsync();
        List<Location> domainLocations = dbLocations.ConvertAll(location =>
        {
            return new Location(location.Name, location.IsActive);
        });
        return domainLocations;
    }
}
