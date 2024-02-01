using MongoDB.Driver;

namespace LagerhotellAPI.Models
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbSettings settings)
        {
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentNullException(nameof(settings), "MongoDbSettings is not configured properly.");


            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _users = database.GetCollection<User>("Users");
        }

        public User Add(string firstName, string lastName, string phoneNumber, string birthDate, string address, string postalCode, string city, string password)
        {
            User user = new(firstName, lastName, phoneNumber, birthDate, address, postalCode, city, password);
            _users.InsertOne(user);
            return user;

        }

        public User? Get(string phoneNumber)
        {
            return _users.Find(User => User.PhoneNumber == phoneNumber).FirstOrDefault();
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
            var user = _users.Find(user => user.Id == id).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public bool DoPasswordsMatch(string password, string requestedPassword)
        {
            return password == requestedPassword;
        }

        public void UpdateUserValues(string firstName, string lastName, string phoneNumber, string birthDate, string password, string address, string postalCode, string city)
        {
            User? user = Get(phoneNumber);
            if (user != null)
            {
                User updatedUser = new User
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    Password = password,
                    Address = address,
                    PostalCode = postalCode,
                    City = city
                };
                _users.ReplaceOne(u => u.Id == updatedUser.Id, updatedUser);

            }
            else
            {
                throw new KeyNotFoundException("Brukeren som skulle oppdateres ble ikke funnet");
            }
        }
    }
}
