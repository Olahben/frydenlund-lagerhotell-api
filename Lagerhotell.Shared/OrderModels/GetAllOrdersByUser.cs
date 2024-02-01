namespace LagerhotellAPI.Models
{
    public class GetAllOrdersByUser
    {
        public class GetAllOrdersByUserRequest
        {
            public required User User { get; set; }
        }

        public class GetAllOrdersByUserResponse
        {
            public List<Order>? Orders { get; set; }
        }
    }
}
