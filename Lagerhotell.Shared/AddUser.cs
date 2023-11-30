using System.Data;

namespace LagerhotellAPI.Models
{
    public class AddUserRequest
    {
        private string firstName { get; set; }
        private string lastName { get; set; }
        private string phoneNumber { get; set; }
        private string birthDate { get; set; }
        private string password { get; set; }


        public string GetFirstName() => firstName;
        public string GetLastName() => lastName;
        public string GetPhoneNumber() => phoneNumber;
        public string GetBirthDate() => birthDate;
        public string GetPassword() => password;
        static public AddUserRequest AddUserRequestFunc(string FirstName, string LastName, string PhoneNumber, string BirthDate, string Password)
        {
            var addUserRequest = new AddUserRequest
            {
                firstName = FirstName,
                lastName = LastName,
                phoneNumber = PhoneNumber,
                birthDate = BirthDate,
                password = Password
            };
            return addUserRequest;
        }
    }
}
