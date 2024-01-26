using System.ComponentModel.DataAnnotations;

namespace LagerhotellAPI.Models
{
    public class AddUserRequest
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string BirthDate { get; set; }

        [Required] public string Address { get; set; }

        [Required] public string PostalCode { get; set; }

        [Required] public string City { get; set; }
        [Required] public string Password { get; set; }

    }
    public class AddUserResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
