namespace LagerhotellAPI.Models
{
    public class GetUserResponse
    {
        public User User { get; set; }

        public User GetUserResponseFunc(string id, string firstName, string lastName, string phoneNumber, string birthDate, string password)
        {
            User = new User(id, firstName, lastName, phoneNumber, birthDate, password);

            return User;
        }
    }
}
