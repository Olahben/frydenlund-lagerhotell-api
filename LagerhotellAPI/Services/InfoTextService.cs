using LagerhotellAPI.Models;
using LagerhotellAPI.Models.ValueTypes;
using MongoDB.Driver;

namespace LagerhotellAPI.Services;

public class InfoTextService
{
    private readonly IMongoCollection<Models.DbModels.InfoText> _infoTexts;

    public InfoTextService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _infoTexts = database.GetCollection<Models.DbModels.InfoText>("InfoTexts");
    }

    public async Task<List<Models.DbModels.InfoText>> GetInfoTexts()
    {
        return await _infoTexts.Find(infoText => true).ToListAsync();
    }

    public async Task<Models.DbModels.InfoText?> GetInfoTextStorageUnit(StorageUnitSizesGroup? sizeGroup)
    {
        if (sizeGroup == null) { return null; }
        return await _infoTexts.Find(infoText => infoText.StorageUnitSizeGroup == sizeGroup).FirstOrDefaultAsync();
    }

    public async Task<string> CreateInfoText(Models.DomainModels.InfoText infoText)
    {
        if (infoText.StorageUnitSizeGroup != null)
        {
            if (await GetInfoTextStorageUnit(infoText.StorageUnitSizeGroup) != null)
            {
                throw new InvalidOperationException("InfoText for this storage unit size group already exists");
            }
        }
        Models.DbModels.InfoText dbInfoText = new() { Text = infoText.Text, Type = infoText.Type, StorageUnitSizeGroup = infoText.StorageUnitSizeGroup };
        await _infoTexts.InsertOneAsync(dbInfoText);
        return dbInfoText.Id;
    }


}
