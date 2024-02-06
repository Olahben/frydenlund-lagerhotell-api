using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("/orders")]

    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // AddOrder
        [HttpPost]
        [Route("add")]
        public IActionResult AddOrder([FromBody] AddOrderRequest request)
        {
            try
            {
                var result = _orderService.AddOrder(request.Order);
                return Ok(new AddOrderResponse { Success = true });
            }
            catch
            {
                return Conflict();
            }
        }

        // DeleteOrder
        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder([FromRoute] string orderId)
        {
            try
            {
                var order = _orderService.DeleteOrder(orderId);
                return Ok(new DeleteOrderResponse { Success = true });
            }
            catch
            {
                return Conflict();
            }
        }

        // GetOrderById
        [HttpGet("{orderId}")]
        public IActionResult GetOrderById([FromRoute] string orderId)
        {
            try
            {
                var order = _orderService.GetOrder(orderId);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch
            {
                return Conflict();
            }
        }

        // GetAllOrders
        [HttpGet]
        public IActionResult GetAllOrders([FromQuery] string? userId, int? skip, int? take)
        {
            // rework logic
            try
            {
                var orders = _orderService.GetAllOrders(userId, skip, take);
                return Ok(orders);
            }
            catch
            {
                return Conflict();
            }
        }

        // GetAllOrdersByUser
        /* [HttpGet("byuser")]
        public IActionResult GetAllOrdersByUser([FromQuery] User user)
        {
            try
            {
                var orders = _orderService.GetAllOrdersByUser(user);
                if (orders != null)
                {
                    return Ok(orders);
                }
                return NotFound();
            }
            catch
            {
                return Conflict();
            }
        } */
    }
}