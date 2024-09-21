global using LagerhotellAPI.Models.DbModels.Auth0;
using LagerhotellAPI.Models;

namespace LagerhotellAPI.Services
{
    public class UserRepository
    {
        private readonly IMongoCollection<Models.DbModels.User> _users;
        private readonly CompanyUserService _companyUserService;
        private readonly Auth0UserService _auth0UserService;

        public UserRepository(MongoDbSettings settings, TokenService tokenService, IConfiguration configuration)
        {
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentNullException(nameof(settings), "MongoDbSettings is not configured properly.");


            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _users = database.GetCollection<Models.DbModels.User>("Users");
            _companyUserService = new CompanyUserService(settings, tokenService);
            _auth0UserService = new Auth0UserService(configuration, settings, tokenService);
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <param name="streetAddress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="password"></param>
        /// <param name="isAdministrator"></param>
        /// <returns>user object</returns>
        public async Task<User> Add(string firstName, string lastName, string phoneNumber, string birthDate, string streetAddress, string postalCode, string city, string password, bool isAdministrator, string email)
        {
            string userId = Guid.NewGuid().ToString();
            Address userAddress = new(streetAddress, postalCode, city);
            Models.DbModels.User user = new(userId, firstName, lastName, phoneNumber, birthDate, userAddress, password, isAdministrator, email, false);
            await _users.InsertOneAsync(user);
            User domainUser = new(user.UserId, user.FirstName, user.LastName, phoneNumber, user.BirthDate, userAddress, user.Password, user.IsAdministrator, email, false);
            await AddUserToAuth0(new User(userId, firstName, lastName, phoneNumber, birthDate, userAddress, password, isAdministrator, email, false));
            return domainUser;

        }

        public async Task AddUserToAuth0(User user)
        {
            UserAuth0 userAuth0 = new(user.Id, user.Email, user.Password);
            await _auth0UserService.AddUser(userAuth0);
        }

        /// <summary>
        /// Gets a user from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>user object</returns>
        public User? Get(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new LagerhotellAPI.Models.DomainModels.User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password, dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified);
        }

        public User? GetByEmail(string email)
        {
            var dbUser = _users.Find(User => User.Email == email).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new LagerhotellAPI.Models.DomainModels.User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password, dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified);
        }

        public async Task<bool> DoesSimilarUserExist(string phoneNumber, string email)
        {
            User user = GetByEmail(email);
            if (user != null)
            {
                return true;
            }
            User user1 = Get(phoneNumber);
            if (user1 != null)
            {
                return true;
            }
            try
            {
                await _companyUserService.GetCompanyUserByEmail(email);
                return true;
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    await _companyUserService.GetCompanyUserByPhoneNumber(phoneNumber);
                    return true;
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a user from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Database user object</returns>
        public Models.DbModels.User? GetByPhoneDbModel(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return dbUser;
        }

        /// <summary>
        /// Gets a users password from the database with the given phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>user password</returns>
        public string? Password(string phoneNumber)
        {
            var user = Get(phoneNumber);
            if (user == null)
            {
                return null;
            }
            return user.Password;
        }

        /// <summary>
        /// Gets a user in the database with the given user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user object</returns>
        public User? GetUserById(string id)
        {
            var dbUser = _users.Find(user => user.UserId == id).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password, dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified);
        }

        /// <summary>
        /// Checks if the given password matches the password in the database
        /// </summary>
        /// <param name="password"></param>
        /// <param name="requestedPassword"></param>
        /// <returns>bool</returns>
        public bool DoPasswordsMatch(string password, string requestedPassword)
        {
            return password == requestedPassword;
        }

        /// <summary>
        /// Updates a users data (except for phone number and Id) in the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <param name="password"></param>
        /// <param name="streetAddress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="isAdministrator"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void UpdateUserValues(string firstName, string lastName, string phoneNumber, string birthDate, string password, string streetAddress, string postalCode, string city, bool isAdministrator, string email)
        {
            Models.DbModels.User oldDbUser = GetByPhoneDbModel(phoneNumber);
            if (oldDbUser != null)
            {
                Address userAddress = new(streetAddress, postalCode, city);
                Models.DbModels.User updatedUserDb = new(oldDbUser.Id, oldDbUser.UserId, firstName, lastName, oldDbUser.PhoneNumber, birthDate, userAddress, password, isAdministrator, email, oldDbUser.IsEmailVerified);
                var filter = Builders<Models.DbModels.User>.Filter.Eq(user => user.UserId, oldDbUser.UserId);
                var options = new ReplaceOptions { IsUpsert = false };
                _users.ReplaceOne(filter, updatedUserDb, options);
            }
            else
            {
                throw new KeyNotFoundException("Brukeren som skulle oppdateres ble ikke funnet");
            }
        }

        public async Task<List<User>> GetAllUsers(int? skip, int? take)
        {
            var dbUsers = await _users.Find(_ => true).Limit(take).Skip(skip).ToListAsync();
            List<User> domainUsers = dbUsers.ConvertAll(dbUser =>
            {
                return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password, dbUser.IsAdministrator, dbUser.Email, dbUser.IsEmailVerified);
            });
            return domainUsers;
        }

        public async Task DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                await _users.DeleteOneAsync(user => user.UserId == id);
            }
            else
            {
                throw new KeyNotFoundException("User not found.");
            }
        }
    }
}
