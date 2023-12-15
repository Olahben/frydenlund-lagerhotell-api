namespace LagerhotellAPI.Models
{
    public class GetUserResponse
    {
        public User User {  get; set; }

        public User GetUserResponseFunc(string firstName, string lastName, string phoneNumber, string birthDate, string password, string id) {
            User.FirstName = firstName;
            User.LastName = lastName;
            User.PhoneNumber = phoneNumber;
            User.BirthDate = birthDate;
            User.Password = password;
            User.Id = id;

            return User;
        }
    }
}
