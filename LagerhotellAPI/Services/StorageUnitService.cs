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

        public async Task AddStorageUnit(StorageUnit storageUnit)
        {
            if (await GetStorageUnitById(storageUnit.Id) == null)
            {
                LagerhotellAPI.Models.DbModels.StorageUnit dbStorageUnit = new LagerhotellAPI.Models.DbModels.StorageUnit { Length = storageUnit.Length, Width = storageUnit.Width, Height = storageUnit.Height, Temperated = storageUnit.Temperated, Area = storageUnit.Area, LockCode = storageUnit.LockCode, Name = storageUnit.Name, Occupied = storageUnit.Occupied, UserId = storageUnit.UserId, Coordinate = storageUnit.Coordinate, PricePerMonth = storageUnit.PricePerMonth };
                dbStorageUnit.StorageUnitId = Guid.NewGuid().ToString();
                await _storageUnits.InsertOneAsync(dbStorageUnit);
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
                LagerhotellAPI.Models.DbModels.StorageUnit updatedDbStorageUnit = new LagerhotellAPI.Models.DbModels.StorageUnit { Id = updatedStorageUnit.Id, Length = updatedStorageUnit.Length, Width = updatedStorageUnit.Width, Height = updatedStorageUnit.Height, Temperated = updatedStorageUnit.Temperated, Area = updatedStorageUnit.Area, LockCode = updatedStorageUnit.LockCode, Name = updatedStorageUnit.Name, Occupied = updatedStorageUnit.Occupied, UserId = updatedStorageUnit.UserId, Coordinate = updatedStorageUnit.Coordinate, PricePerMonth = updatedStorageUnit.PricePerMonth };
                await _storageUnits.ReplaceOneAsync(unit => unit.Id == storageUnitId, updatedDbStorageUnit);
            }
            throw new KeyNotFoundException();
        }

        public async Task<StorageUnit> GetStorageUnitById(string storageUnitId)
        {
            var dbStorageUnit = await _storageUnits.Find(unit => unit.Id == storageUnitId).FirstOrDefaultAsync();
            var domainStorageUnit = new LagerhotellAPI.Models.DomainModels.StorageUnit { Id = dbStorageUnit.Id, Length = dbStorageUnit.Length, Width = dbStorageUnit.Width, Height = dbStorageUnit.Height, Temperated = dbStorageUnit.Temperated, Area = dbStorageUnit.Area, LockCode = dbStorageUnit.LockCode, Name = dbStorageUnit.Name, Occupied = dbStorageUnit.Occupied, UserId = dbStorageUnit.UserId, Coordinate = dbStorageUnit.Coordinate, PricePerMonth = dbStorageUnit.PricePerMonth };
            return domainStorageUnit;
        }

        public async Task<List<LagerhotellAPI.Models.DomainModels.StorageUnit>> GetAllStorageUnits(int? skip, int? take)
        {
            List<LagerhotellAPI.Models.DbModels.StorageUnit> dbStorageUnits = await _storageUnits.Find(_ => true).Limit(take).Skip(skip).ToListAsync();
            List<LagerhotellAPI.Models.DomainModels.StorageUnit> domainStorageUnits = dbStorageUnits.ConvertAll(dbStorageUnit =>
            {
                return new LagerhotellAPI.Models.DomainModels.StorageUnit
                {
                    Id = dbStorageUnit.StorageUnitId,
                    Length = dbStorageUnit.Length,
                    Width = dbStorageUnit.Width,
                    Height = dbStorageUnit.Height,
                    Temperated = dbStorageUnit.Temperated,
                    Area = dbStorageUnit.Area,
                    LockCode = dbStorageUnit.LockCode,
                    Name = dbStorageUnit.Name,
                    Occupied = dbStorageUnit.Occupied,
                    UserId = dbStorageUnit.UserId,
                    Coordinate = dbStorageUnit.Coordinate,
                    PricePerMonth = dbStorageUnit.PricePerMonth,
                };
            });
            return domainStorageUnits;
        }
    }
}
