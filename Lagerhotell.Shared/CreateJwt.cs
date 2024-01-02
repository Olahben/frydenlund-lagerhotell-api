namespace LagerhotellAPI.Models
{
    public class CreateJwt
    {
        public class CreateJwtRequestService
        {
            public string PhoneNumber { get; set; }
        }

        public class CreateJwtRequest
        {
            public string Id { get; set; }
        }

        public class CreateJwtResponse
        {
            public string JWT { get; set; }
        }
    }
}
