namespace LagerhotellAPI.Models
{
    public class CreateJwt
    {
        public class CreateJwtRequest
        {
            public string PhoneNumber;
        }

        public class CreateJwtResponse
        {
            public string JWT;
        }
    }
}
