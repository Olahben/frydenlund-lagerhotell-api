using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class StorageUnitService
    {
        private readonly IMongoCollection<StorageUnit> _storageUnits;

        public StorageUnitService(IMongoDatabase database)
        {
            _storageUnits = database.GetCollection<StorageUnit>("StorageUnits");
        }

        public async Task AddStorageUnit(StorageUnit storageUnit)
        {
            if (await GetStorageUnitById(storageUnit.Id) == null)
            {
                await _storageUnits.InsertOneAsync(storageUnit);
            }
            throw new InvalidOperationException("Already exists");
        }

        public async Task DeleteStorageUnit(string storageUnitId)
        {
            if (await GetStorageUnitById(storageUnitId) != null)
            {
                await _storageUnits.DeleteOneAsync(unit => unit.Id == storageUnitId);
            }
            throw new KeyNotFoundException();
        }
        public async Task ModifyStorageUnit(string storageUnitId, StorageUnit updatedStorageUnit)
        {
            if (await GetStorageUnitById(storageUnitId) != null)
            {
                await _storageUnits.ReplaceOneAsync(unit => unit.Id == storageUnitId, updatedStorageUnit);
            }
            throw new KeyNotFoundException();
        }

        public async Task<StorageUnit> GetStorageUnitById(string storageUnitId)
        {
            return await _storageUnits.Find(unit => unit.Id == storageUnitId).FirstOrDefaultAsync();
        }

        public async Task<List<StorageUnit>> GetAllStorageUnits()
        {
            return await _storageUnits.Find(_ => true).ToListAsync();
        }
    }
}
