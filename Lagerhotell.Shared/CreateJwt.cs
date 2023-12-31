namespace LagerhotellAPI.Models
{
    public class CreateJwt
    {
        public class CreateJwtRequest
        {
            public string PhoneNumber { get; set; }
        }

        public class CreateJwtResponse
        {
            public string JWT { get; set; }
        }
    }
}
