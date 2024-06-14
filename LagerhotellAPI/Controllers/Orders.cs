using LagerhotellAPI.Models.DomainModels.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("/orders")]

    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private OrderValidator _orderValidator;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
            _orderValidator = new OrderValidator();
        }

        /// <summary>
        /// Adds an order to the database, and returns the orderId
        /// </summary>
        /// <param name="request"></param>
        /// <returns>orderId</returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            var validationResult = _orderValidator.Validate(request.Order);
            if (validationResult.IsValid)
            {
                string orderId = await _orderService.AddOrder(request.Order);
                return Ok(new AddOrderResponse(orderId));
            }
            else
            {
                Console.WriteLine($"The request order did not meet the validators requirements, {validationResult.Errors}");
                return BadRequest($"The request order did not meet the validators requirements, {validationResult.Errors}");
            }
        }

        /// <summary>
        /// Deletes an order with the given orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Success bool</returns>
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

        /// <summary>
        /// Gets an order with the given orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Order domain object</returns>
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

        /// <summary>
        /// Gets all orders in the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>A list of all orders</returns>
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