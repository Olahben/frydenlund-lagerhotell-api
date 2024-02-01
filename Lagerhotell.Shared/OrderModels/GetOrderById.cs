namespace LagerhotellAPI.Models
{
    public class GetOrderById
    {
        public class GetOrderByIdRequest
        {
            public required string Id { get; set; }
        }

        public class GetOrderByIdResponse
        {
            public required Order Order { get; set; }
        }
    }
}
