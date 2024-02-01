namespace LagerhotellAPI.Models
{
    public class DeleteOrder
    {
        public class DeleteOrderRequest
        {
            public required string OrderId { get; set; }
        }
        public class DeleteOrderResponse
        {
            public required bool Success { get; set; }
        }
    }
}
