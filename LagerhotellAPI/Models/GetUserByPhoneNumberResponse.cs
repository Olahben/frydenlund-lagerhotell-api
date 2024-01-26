namespace LagerhotellAPI.Models
{
    public class GetUserByPhoneNumberResponse
    {
        public User User { get; set; }

        public User GetUserByPhoneNumberResponseFunc(string id, string firstName, string lastName, string phoneNumber, string birthDate, string password)
        {
            User = new User(id, firstName, lastName, phoneNumber, birthDate, password);
            return User;
        }
    }
}
