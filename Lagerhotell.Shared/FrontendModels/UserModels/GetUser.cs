namespace LagerhotellAPI.Models.FrontendModels;


public class GetUser
{
    public class GetUserRequest
    {
        public required string UserId { get; set; }
    }

    public class GetUserResponse
    {
        public User? User { get; set; }

        public User GetUserResponseFunc(string id, string firstName, string lastName, string phoneNumber, string birthDate, string streetAddress, string postalCode, string city, string password, bool isAdministrator)
        {
            Address userAddress = new(streetAddress, postalCode, city);
            User = new User(id, firstName, lastName, phoneNumber, birthDate, userAddress, password, isAdministrator);

            return User;
        }
    }
}
