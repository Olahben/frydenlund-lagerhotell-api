using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class StorageUnitService
    {
        private readonly IMongoCollection<LagerhotellAPI.Models.DbModels.StorageUnit> _storageUnits;

        public StorageUnitService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _storageUnits = database.GetCollection<LagerhotellAPI.Models.DbModels.StorageUnit>("StorageUnits");
        }

        public async Task<string> AddStorageUnit(StorageUnit storageUnit)
        {
            string storageUnitId = Guid.NewGuid().ToString();
            LagerhotellAPI.Models.DbModels.StorageUnit dbStorageUnit = new(storageUnitId, storageUnit.Dimensions, storageUnit.Temperated, storageUnit.LockCode, storageUnit.Name, storageUnit.Occupied, storageUnit.UserId, storageUnit.Coordinate, storageUnit.PricePerMonth);
            await _storageUnits.InsertOneAsync(dbStorageUnit);
            return storageUnitId;
        }

        public async Task DeleteStorageUnit(string storageUnitId)
        {
            if (await GetStorageUnitById(storageUnitId) != null)
            {
                await _storageUnits.DeleteOneAsync(unit => unit.StorageUnitId == storageUnitId);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public async Task ModifyStorageUnit(string storageUnitId, StorageUnit updatedStorageUnit)
        {
            Models.DbModels.StorageUnit oldStorageUnit = await GetStorageUnitByIdDbModel(storageUnitId);
            if (oldStorageUnit != null)
            {
                Models.DbModels.StorageUnit updatedDbStorageUnit = new(oldStorageUnit.Id, updatedStorageUnit.StorageUnitId, updatedStorageUnit.Dimensions, updatedStorageUnit.Temperated, updatedStorageUnit.LockCode, updatedStorageUnit.Name, updatedStorageUnit.Occupied, updatedStorageUnit.UserId, updatedStorageUnit.Coordinate, updatedStorageUnit.PricePerMonth);
                await _storageUnits.ReplaceOneAsync(unit => unit.StorageUnitId == storageUnitId, updatedDbStorageUnit);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<StorageUnit> GetStorageUnitById(string storageUnitId)
        {
            var dbStorageUnit = await _storageUnits.Find(unit => unit.StorageUnitId == storageUnitId).FirstOrDefaultAsync();
            LagerhotellAPI.Models.DomainModels.StorageUnit domainStorageUnit = new(dbStorageUnit.Id, dbStorageUnit.Dimensions, dbStorageUnit.Temperated, dbStorageUnit.LockCode, dbStorageUnit.Name, dbStorageUnit.Occupied, dbStorageUnit.UserId, dbStorageUnit.Coordinate, dbStorageUnit.PricePerMonth);
            return domainStorageUnit;
        }
        public async Task<Models.DbModels.StorageUnit> GetStorageUnitByIdDbModel(string storageUnitId)
        {
            var dbStorageUnit = await _storageUnits.Find(unit => unit.StorageUnitId == storageUnitId).FirstOrDefaultAsync();
            return dbStorageUnit;
        }

        public async Task<List<LagerhotellAPI.Models.DomainModels.StorageUnit>> GetAllStorageUnits(int? skip, int? take)
        {
            List<LagerhotellAPI.Models.DbModels.StorageUnit> dbStorageUnits = await _storageUnits.Find(_ => true).Limit(take).Skip(skip).ToListAsync();
            List<LagerhotellAPI.Models.DomainModels.StorageUnit> domainStorageUnits = dbStorageUnits.ConvertAll(dbStorageUnit =>
            {

                return new LagerhotellAPI.Models.DomainModels.StorageUnit(dbStorageUnit.StorageUnitId, dbStorageUnit.Dimensions, dbStorageUnit.Temperated, dbStorageUnit.LockCode, dbStorageUnit.Name, dbStorageUnit.Occupied, dbStorageUnit.UserId, dbStorageUnit.Coordinate, dbStorageUnit.PricePerMonth);
            });
            return domainStorageUnits;
        }
    }
}
