using ElectroKart.Common.DTOS;
using ElectroKart.Common.Messages;
using ElectroKart.Common.Models;
using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly OrdersService _orderService;
        public OrdersController(ILogger<OrdersController> logger,OrdersService ordersService)
        {
            _logger = logger;
            _orderService = ordersService;
        }
        /// <summary>
        /// Creates a new customer order by processing the order details received in the request body.
        /// Validates inventory availability, places the order, updates inventory,
        /// and returns an appropriate HTTP response based on the outcome.
        /// </summary>
        /// <param name="placeOrderDTO">The order details, including customer ID and list of products with quantities.</param>
        [HttpPost("placeOrder")]
        public async Task<IActionResult> CreateCustomerOrderAsync([FromBody]PlaceOrderDTO placeOrderDTO)
        {
            try
            {
                var response = await _orderService.CreateCustomerOrder(placeOrderDTO);
                if (response == 0)
                {
                    return NotFound(OrderProductMessages.OutOfStock);
                }
                else if(response == 1)
                {
                    return Ok(OrderProductMessages.OrderSuccess);
                }
                else if (response == 2)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, OrderProductMessages.OrderError);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, OrderProductMessages.OrderError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the customer order.");
                return StatusCode(StatusCodes.Status500InternalServerError, OrderProductMessages.OrderError);
            }
        }
        [HttpGet("GetOrdersByCustomerId/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync([FromRoute] int customerId)
        {
            try
            {
                var response = await _orderService.GetOrdersByCustomerId(customerId);
                if (response.StatusCode != 2)
                {
                    return NotFound(response.Message);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the orders for customer ID {customerId}");
                return StatusCode(StatusCodes.Status500InternalServerError, OrderProductMessages.OrderError);
            }
        }
        //[HttpPut("CancelOrder")]
        //public async Task<IActionResult> CancelOrderAsync()
        //{

        //}
    }
}
