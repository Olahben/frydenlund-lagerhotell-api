using LagerhotellAPI.Models;
using LagerhotellAPI.Models.ValueTypes;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class UserRepository
    {
        private readonly IMongoCollection<Models.DbModels.User> _users;

        public UserRepository(MongoDbSettings settings)
        {
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentNullException(nameof(settings), "MongoDbSettings is not configured properly.");


            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _users = database.GetCollection<Models.DbModels.User>("Users");
        }

        public User Add(string firstName, string lastName, string phoneNumber, string birthDate, string streetAddress, string postalCode, string city, string password)
        {
            string userId = Guid.NewGuid().ToString();
            Address userAddress = new(streetAddress, postalCode, city);
            Models.DbModels.User user = new(userId, firstName, lastName, phoneNumber, birthDate, userAddress, password);
            _users.InsertOne(user);
            User domainUser = new(user.Id, user.FirstName, user.LastName, phoneNumber, user.BirthDate, userAddress, user.Password);
            return domainUser;

        }

        public User? Get(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new LagerhotellAPI.Models.DomainModels.User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password);
        }

        public string? Password(string phoneNumber)
        {
            var user = Get(phoneNumber);
            // Handle if user is null
            if (user == null)
            {
                return null;
            }
            return user.Password;
        }

        public User? GetUserById(string id)
        {
            var dbUser = _users.Find(user => user.Id == id).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new User(dbUser.UserId, dbUser.FirstName, dbUser.LastName, dbUser.PhoneNumber, dbUser.BirthDate, dbUser.Address, dbUser.Password);
        }

        public bool DoPasswordsMatch(string password, string requestedPassword)
        {
            return password == requestedPassword;
        }

        public void UpdateUserValues(string firstName, string lastName, string phoneNumber, string birthDate, string password, string streetAddress, string postalCode, string city)
        {
            User? domainUser = Get(phoneNumber);
            if (domainUser != null)
            {
                Address userAddress = new(streetAddress, postalCode, city);
                Models.DbModels.User updatedUserDb = new(domainUser.Id, firstName, lastName, phoneNumber, birthDate, userAddress, password);
                _users.ReplaceOne(u => u.Id == updatedUserDb.Id, updatedUserDb);

            }
            else
            {
                throw new KeyNotFoundException("Brukeren som skulle oppdateres ble ikke funnet");
            }
        }
    }
}
