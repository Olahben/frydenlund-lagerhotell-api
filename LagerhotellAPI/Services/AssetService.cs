using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services;

public class AssetService
{
    private readonly IMongoCollection<Models.DbModels.ImageAsset> _assets;
    public AssetService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _assets = database.GetCollection<Models.DbModels.ImageAsset>("Assets");
    }
}
