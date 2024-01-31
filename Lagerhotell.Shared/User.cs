﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public User(string id, string firstName, string lastName, string phoneNumber, string birthDate, string address, string postalCode, string city, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Password = password;
        }

        public User(string firstName, string lastName, string phoneNumber, string birthDate, string address, string postalCode, string city, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Password = password;
        }
        public User()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
    }
}
