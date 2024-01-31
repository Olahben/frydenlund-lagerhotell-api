namespace LagerhotellAPI.Models
{
    public class Login
    {
        public class LoginRequest
        {
            public required string PhoneNumber { get; set; }
            public required string Password { get; set; }
        }

        public class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
