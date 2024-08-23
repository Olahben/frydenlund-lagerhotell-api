global using MongoDB.Driver;
using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;
using System.Data.SqlTypes;

namespace LagerhotellAPI.Services
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly IMongoCollection<Models.DbModels.CompanyUserDocument> _companyUsers;

        public CompanyUserService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _companyUsers = database.GetCollection<CompanyUserDocument>("CompanyUsers");
        }

        public async Task<CompanyUserDocument> GetCompanyUserAsync(string id)
        {
            return await _companyUsers.Find(cu => cu.CompanyUserId == id).FirstOrDefaultAsync();
        }

        public async Task<List<CompanyUserDocument>> GetCompanyUsersAsync(int? take, int? skip)
        {
            var query = _companyUsers.Find(cu => true);
            if (take.HasValue)
            {
                query = query.Limit(take.Value);
            }
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<CompanyUser> CreateCompanyUserAsync(CompanyUser companyUser)
        {
            if (await GetCompanyUserAsync(companyUser.CompanyUserId) != null)
            {
                throw new SqlAlreadyFilledException("Company user already exists");
            }
            var companyUserDocument = new CompanyUserDocument(companyUser.CompanyUserId, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address);
            await _companyUsers.InsertOneAsync(companyUserDocument);
            return companyUser;
        }

        public async Task UpdateCompanyUserAsync(string id, CompanyUser companyUser)
        {
            if (await GetCompanyUserAsync(id) == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            var companyUserDocument = new CompanyUserDocument(companyUser.CompanyUserId, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address);
            await _companyUsers.ReplaceOneAsync(cu => cu.CompanyUserId == id, companyUserDocument);
        }

        public async Task DeleteCompanyUserAsync(string id)
        {
            if (await GetCompanyUserAsync(id) == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            await _companyUsers.DeleteOneAsync(cu => cu.CompanyUserId == id);
        }
    }
}
