using Newtonsoft.Json;

namespace LagerhotellAPI.Models
{
    public class UserRepository
    {
        private List<User> _users;
        private readonly string _filePath = @"C:\Users\ohage\SKOLE\Programmering\Lagerhotell\wwwroot\Data\users.json";
        public List<User> Users { get {
                if (_users == null)
                {
                    Load();
                }
                return _users;
            }
        }

        public string Add(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            Users.Add(user);
            // Ensure JSON is saved
            Save();
            return user.Id;
            
        }

        public User? Get(string phoneNumber)
        {
            return Users.Where(_ => _.PhoneNumber == phoneNumber).SingleOrDefault();
        }
        private void Save() 
        {
            var updatedJson = JsonConvert.SerializeObject(Users);
            System.IO.File.WriteAllText(_filePath, updatedJson);
        }
        private void Load()
        {
            // Check if JSON is read
            string existingJson = System.IO.File.ReadAllText(_filePath);
            _users = JsonConvert.DeserializeObject<List<User>>(existingJson);
        }
    }
}
