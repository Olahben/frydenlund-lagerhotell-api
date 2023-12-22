namespace LagerhotellAPI.Models
{
    public class User
    {
        public User(string id, string firstName, string lastName, string phoneNumber, string birthDate, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Password = password;
            Id = id;
        }
        public User()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
    }
}
