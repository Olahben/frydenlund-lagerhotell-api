using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class CompanyUserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string CompanyUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string CompanyNumber { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public Address Address { get; set; }

    public CompanyUserDocument(string companyUserId, string firstName, string lastName, string name, string companyNumber, string email, string phoneNumber, Address address, string password, bool isEmailVerified)
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
        IsEmailVerified = isEmailVerified;
    }
    public CompanyUserDocument(string id, string companyUserId, string firstName, string lastName, string name, string companyNumber, string email, string phoneNumber, Address address, string password, bool isEmailVerified)
    {
        Id = id;
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Password = password;
        IsEmailVerified = isEmailVerified;
    }
}