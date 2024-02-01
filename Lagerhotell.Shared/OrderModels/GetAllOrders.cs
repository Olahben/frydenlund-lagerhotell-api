namespace LagerhotellAPI.Models
{
    public class GetAllOrders
    {
        public class GetAllOrdersRequest
        {

        }

        public class GetAllOrdersResponse
        {
            public required List<Order> Orders { get; set; }
        }
    }
}
