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

    /// <summary>
    /// Gets all InfoTexts
    /// </summary>
    /// <returns></returns>
    public async Task<List<Models.DbModels.InfoText>> GetInfoTexts(int? skip, int? take)
    {
        return await _infoTexts.Find(infoText => true).Limit(take).Skip(skip).ToListAsync();
    }

    /// <summary>
    /// Gets the InfoText with the given storage unit size group
    /// </summary>
    /// <param name="sizeGroup"></param>
    /// <returns></returns>
    public async Task<Models.DbModels.InfoText?> GetInfoTextStorageUnit(StorageUnitSizesGroup? sizeGroup)
    {
        if (sizeGroup == null) { return null; }
        return await _infoTexts.Find(infoText => infoText.StorageUnitSizeGroup == sizeGroup).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Creates a new InfoText with the given InfoText and assigns a new InfoTextId
    /// </summary>
    /// <param name="infoText"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> CreateInfoText(Models.DomainModels.InfoText infoText)
    {
        if (infoText.StorageUnitSizeGroup != null)
        {
            if (await GetInfoTextStorageUnit(infoText.StorageUnitSizeGroup) != null)
            {
                throw new InvalidOperationException("InfoText for this storage unit size group already exists");
            }
        }
        Models.DbModels.InfoText dbInfoText = new() { Text = infoText.Text, Type = infoText.Type, StorageUnitSizeGroup = infoText.StorageUnitSizeGroup, InfoTextId = Guid.NewGuid().ToString() };
        await _infoTexts.InsertOneAsync(dbInfoText);
        return dbInfoText.Id;
    }
    /// <summary>
    /// Modifies a InfoText with given InfoTextId with the given InfoText
    /// </summary>
    /// <param name="infoTextId"></param>
    /// <param name="infoText"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ModifyInfoText(string infoTextId, Models.DomainModels.InfoText infoText)
    {
        Models.DbModels.InfoText dbInfoText = await _infoTexts.Find(infoText => infoText.InfoTextId == infoTextId).FirstOrDefaultAsync();
        if (dbInfoText == null)
        {
            throw new InvalidOperationException("InfoText with given InfoTextId does not exist");
        }
        dbInfoText.Text = infoText.Text;
        dbInfoText.Type = infoText.Type;
        dbInfoText.StorageUnitSizeGroup = infoText.StorageUnitSizeGroup;

        await _infoTexts.ReplaceOneAsync(infoText => infoText.InfoTextId == infoTextId, dbInfoText);
    }

}
