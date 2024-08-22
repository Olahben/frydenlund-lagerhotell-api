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
    public int CompanyNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Address Address { get; set; }

    public CompanyUserDocument(string companyUserId, string firstName, string lastName, string name, int companyNumber, string email, string phoneNumber, Address address)
    {
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }
}