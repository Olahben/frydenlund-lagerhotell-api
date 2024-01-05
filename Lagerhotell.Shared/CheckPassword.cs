namespace LagerhotellAPI.Models
{
    public class CheckPassword
    {
        public class CheckPasswordRequest
        {
            public required string PhoneNumber { get; set; }
            public required string Password { get; set; }
        }

        public class CheckPasswordResponse
        {
            public string Token { get; set; }
        }
    }
}
