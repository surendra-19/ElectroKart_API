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
        //[HttpPost("AddToCart")]
        //public async Task<IActionResult> AddToCartAsync([FromBody] CartItemDTO cartItemDTO)
        //{
        //    try
        //    {
        //        var result = await _cartService.AddToCart(cartItemDTO);
        //        if (result)
        //        {
        //            return Ok("Item added to cart successfully.");
        //        }
        //        else
        //        {
        //            return BadRequest("Failed to add item to cart.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding item to cart.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
        [HttpDelete("RemoveFromCart/{customerId}/{itemId}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] int itemId, int customerId)
        {
            try
            {
                var result = await _cartService.RemoveFromCart(customerId,itemId);
                if (result)
                {
                    return Ok("Item removed from cart successfully.");
                }
                else
                {
                    return BadRequest("Failed to remove item from cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing item from cart.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
