using ElectroKart.Common.DTOS;
using ElectroKart.Common.Messages;
using ElectroKart.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly CartService _cartService;
        public CartController(CartService cartService, ILogger<CartController> logger)
        {
            _logger = logger;
            _cartService = cartService;
        }
        /// <summary>
        /// Retrieves all items in the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>List of items in the cart.</returns>
        [HttpGet("GetCartItems/{customerId}")]
        public async Task<IActionResult> GetCartItemsAsync([FromRoute] int customerId)
        {
            try
            {
                int response = await _cartService.GetCartItems(customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting cart items for user:{customerId}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Removes a specific item from the customer's cart.
        /// </summary>
        /// <param name="itemId">The ID of the item to remove.</param>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>200 OK if removed, 404 Not Found if item doesn't exist.</returns>
        [HttpPut("RemoveFromCart/{customerId}/{itemId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] int itemId, int customerId)
        {
            try
            {
                int result = await _cartService.RemoveFromCart(customerId:customerId, itemId:itemId);
                if (result == 1)
                {
                    return Ok("Item removed from cart successfully.");
                }
                else if (result == 2)
                {
                    return Ok("Cart cleared successfully.");
                }
                else
                {
                    return NotFound("Item not found in cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while removing {itemId} from cart.");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Clears all items from the customer's cart.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>200 OK if successful, 400 Bad Request if failed.</returns>
        [HttpDelete("ClearCart/{customerId}")]
        public async Task<IActionResult> ClearCartAsync([FromRoute] int customerId)
        {
            try
            {
                var result = await _cartService.ClearCart(customerId);
                if (result)
                {
                    return Ok("Cart cleared successfully.");
                }
                else
                {
                    return BadRequest("Failed to clear cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while clearing cart for user:{customerId}");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Adds an item to the customer's cart.
        /// </summary>
        /// <param name="cartItemDTO">Details of the item to add.</param>
        /// <returns>200 OK if added successfully, 400 Bad Request if failed.</returns>
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCartAsync([FromBody] CartItemDTO cartItemDTO)
        {
            try
            {
                var result = await _cartService.AddToCart(cartItemDTO);
                if (result)
                {
                    return Ok("Item added to cart successfully.");
                }
                else
                {
                    return BadRequest("Failed to add item to cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding item to cart.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
