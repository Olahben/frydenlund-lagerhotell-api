using LagerhotellAPI.Models;
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

        public User Add(string firstName, string lastName, string phoneNumber, string birthDate, string address, string postalCode, string city, string password)
        {
            string userId = Guid.NewGuid().ToString();
            Models.DbModels.User user = new(userId, firstName, lastName, phoneNumber, birthDate, address, postalCode, city, password);
            _users.InsertOne(user);
            User domainUser = new(user.Id, user.FirstName, user.LastName, phoneNumber, user.BirthDate, address, postalCode, user.City, user.Password);
            return domainUser;

        }

        public User? Get(string phoneNumber)
        {
            var dbUser = _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
            if (dbUser == null)
            {
                return null;
            }
            return new LagerhotellAPI.Models.DomainModels.User { Address = dbUser.Address, PostalCode = dbUser.PostalCode, PhoneNumber = dbUser.PhoneNumber, BirthDate = dbUser.BirthDate, City = dbUser.City, Password = dbUser.Password };
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
            return new LagerhotellAPI.Models.DomainModels.User { Id = dbUser.UserId, FirstName = dbUser.FirstName, LastName = dbUser.LastName, PhoneNumber = dbUser.PhoneNumber, BirthDate = dbUser.BirthDate, Password = dbUser.Password, Address = dbUser.Address, PostalCode = dbUser.PostalCode, City = dbUser.City };
        }

        public bool DoPasswordsMatch(string password, string requestedPassword)
        {
            return password == requestedPassword;
        }

        public void UpdateUserValues(string firstName, string lastName, string phoneNumber, string birthDate, string password, string address, string postalCode, string city)
        {
            User? domainUser = Get(phoneNumber);
            if (domainUser != null)
            {
                Models.DbModels.User updatedUserDb = new Models.DbModels.User
                {
                    Id = domainUser.Id,
                    PhoneNumber = domainUser.PhoneNumber,
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    Password = password,
                    Address = address,
                    PostalCode = postalCode,
                    City = city
                };
                _users.ReplaceOne(u => u.Id == updatedUserDb.Id, updatedUserDb);

            }
            else
            {
                throw new KeyNotFoundException("Brukeren som skulle oppdateres ble ikke funnet");
            }
        }
    }
}
