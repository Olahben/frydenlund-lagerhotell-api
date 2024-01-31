namespace LagerhotellAPI.Models
{

    public class UpdateUserValuesRequest
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }

        public required string BirthDate { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
    }

    public class UpdateUserValuesResponse
    {
        public required bool Success { get; set; }
    }
}
