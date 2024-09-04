global using MongoDB.Driver;
using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;
using System.Data.SqlTypes;

namespace LagerhotellAPI.Services
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly IMongoCollection<Models.DbModels.CompanyUserDocument> _companyUsers;
        private readonly TokenService _tokenService;

        public CompanyUserService(MongoDbSettings settings, TokenService tokenService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _companyUsers = database.GetCollection<CompanyUserDocument>("CompanyUsers");
            _tokenService = tokenService;
        }

        public async Task<CompanyUser> GetCompanyUserAsync(string id)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(cu => cu.CompanyUserId == id).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            return new CompanyUser(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, dbCompanyUserDocument.Password);
        }

        public async Task<CompanyUserDocument> GetCompanyUserDocument(string id)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(cu => cu.CompanyUserId == id).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            return dbCompanyUserDocument;
        }

        public async Task<List<CompanyUser>> GetCompanyUsersAsync(int? take, int? skip)
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
            List<CompanyUserDocument> dbCompanyUsersDocuments = await query.ToListAsync();
            return dbCompanyUsersDocuments.Select(cu => new CompanyUser(cu.CompanyUserId, cu.FirstName, cu.LastName, cu.Name, cu.CompanyNumber, cu.Email, cu.PhoneNumber, cu.Address, cu.Password)).ToList();
        }

        public async Task<CompanyUser> GetCompanyUserByPhoneNumber(string phoneNumber)
        {
            var dbCompanyUserDocument = await _companyUsers.Find(cu => cu.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, dbCompanyUserDocument.Password);
            return companyUser;
        }

        public async Task<CompanyUser> GetCompanyUserByEmail(string email)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(c => c.Email == email).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, dbCompanyUserDocument.Password);
            return companyUser;
        }

        public async Task<CompanyUser> GetCompanyUserByCompanyNumber(string companyNumber)
        {
            CompanyUserDocument dbCompanyUserDocument = await _companyUsers.Find(c => c.CompanyNumber == companyNumber).FirstOrDefaultAsync();
            if (dbCompanyUserDocument == null)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            CompanyUser companyUser = new(dbCompanyUserDocument.CompanyUserId, dbCompanyUserDocument.FirstName, dbCompanyUserDocument.LastName, dbCompanyUserDocument.Name, dbCompanyUserDocument.CompanyNumber, dbCompanyUserDocument.Email, dbCompanyUserDocument.PhoneNumber, dbCompanyUserDocument.Address, dbCompanyUserDocument.Password);
            return companyUser;
        }
        public async Task<(string, string)> CreateCompanyUserAsync(CompanyUser companyUser)
        {
            try
            {
                await GetCompanyUserByPhoneNumber(companyUser.PhoneNumber);
                await GetCompanyUserByEmail(companyUser.Email);
                await GetCompanyUserByCompanyNumber(companyUser.CompanyNumber);
                throw new SqlAlreadyFilledException("Company user already exists");
            }
            catch (KeyNotFoundException)
            {
                string id = Guid.NewGuid().ToString();
                var companyUserDocument = new CompanyUserDocument(id, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address, companyUser.Password);
                await _companyUsers.InsertOneAsync(companyUserDocument);
                // User is not administrator so third parameter is false
                string userAccessToken = _tokenService.CreateJwt(id, companyUser.PhoneNumber, false).Token;
                return (id, userAccessToken);
            }
        }

        public async Task UpdateCompanyUserAsync(string id, CompanyUser companyUser)
        {
            CompanyUserDocument existingCompanyUser;
            try
            {
                existingCompanyUser = await GetCompanyUserDocument(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            var companyUserDocument = new CompanyUserDocument(existingCompanyUser.Id, existingCompanyUser.CompanyUserId, companyUser.FirstName, companyUser.LastName, companyUser.Name, companyUser.CompanyNumber, companyUser.Email, companyUser.PhoneNumber, companyUser.Address, companyUser.Password);
            await _companyUsers.ReplaceOneAsync(cu => cu.CompanyUserId == id, companyUserDocument);
        }

        public async Task DeleteCompanyUserAsync(string id)
        {
            try
            {
                var companyUser = await GetCompanyUserAsync(id);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Company user not found");
            }
            await _companyUsers.DeleteOneAsync(cu => cu.CompanyUserId == id);
        }
    }
}
