namespace LagerhotellAPI.Models
{
    public class DecodeJwt
    {
        public class DecodeJwtResponse
        {
            public string PhoneNumber { get; set; }
        }
        public class DecodeJwtRequest
        {
            public string Token { get; set; }
        }
    }
}
