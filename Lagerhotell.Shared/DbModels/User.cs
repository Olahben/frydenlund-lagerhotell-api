

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

[BsonIgnoreExtraElements]
public class User
{

    public User(string id, string userId, string firstName, string lastName, string phoneNumber, string birthDate, Address address, string password, bool isAdministrator)
    {
        Id = id;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
        Address = address;
        Password = password;
        IsAdministrator = isAdministrator;
    }
    public User(string userId, string firstName, string lastName, string phoneNumber, string birthDate, Address address, string password, bool isAdministrator)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
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

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string BirthDate { get; set; }

    public Address Address { get; set; }
    public string Password { get; set; }
    public bool IsAdministrator { get; set; }
}