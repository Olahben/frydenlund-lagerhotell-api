namespace LagerhotellAPI.Models.DomainModels;

public class CompanyUser
{
    public string CompanyUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public int CompanyNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public Address Address { get; set; }

    public CompanyUser(string companyUserId, string firstName, string lastName, string name, int companyNumber, string email, string phoneNumber, Address address, string password)
    {
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Password = password;
    }

    public CompanyUser()
    {
        // empty ctor
        Address = new Address();
    }
};
