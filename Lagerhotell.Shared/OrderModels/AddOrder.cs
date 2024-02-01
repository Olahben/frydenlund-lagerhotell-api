namespace LagerhotellAPI.Models
{
    public class AddOrder
    {
        public class AddOrderRequest
        {
            public required Order Order { get; set; }
        }
        public class AddOrderResponse
        {
            public required bool Success { get; set; }

        }
    }
}
