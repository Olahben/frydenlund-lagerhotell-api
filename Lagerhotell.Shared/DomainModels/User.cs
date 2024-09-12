namespace LagerhotellAPI.Models.DomainModels;
public class User
{
    public User(string userId, string firstName, string lastName, string phoneNumber, string birthDate, Address address, string password, bool isAdministrator, string email)
    {
        Id = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BirthDate = birthDate;
        Address = address;
        Password = password;
        IsAdministrator = isAdministrator;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BirthDate { get; set; }

    public Address Address { get; set; }
    public string Password { get; set; }

    public bool IsAdministrator { get; set; }
}
