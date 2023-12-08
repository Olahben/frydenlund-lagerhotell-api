using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace LagerhotellAPI.Models
{
    public class AddUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public string Password { get; set; }

        static public AddUserRequest AddUserRequestFunc(string firstName, string lastName, string phoneNumber, string birthDate, string password)
        {
            var addUserRequest = new AddUserRequest
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                BirthDate = birthDate,
                Password = password
            };
            return addUserRequest;
        }

    }
    public class AddUserResponse 
    { 
        public string UserId { get; set; } 
    }
}
