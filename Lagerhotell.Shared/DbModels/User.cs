

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.DbModels;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    // Should be generated upon registation
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string BirthDate { get; set; }

    public required string Address { get; set; }
    public required string PostalCode { get; set; }
    public required string City { get; set; }
    public required string Password { get; set; }
}
