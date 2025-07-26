using ElectroKart.Common.Messages;
using ElectroKart.Common.Models;
using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsService _productsService;
        public ProductsController(ProductsService productService,ILogger<ProductsController> logger)
        {
            _productsService = productService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = await _productsService.GetAllProducts();
                return Ok(new {
                    Data = products,
                    Message = RetrieveProductsMessages.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, RetrieveProductsMessages.Failed);
                return StatusCode(StatusCodes.Status500InternalServerError, RetrieveProductsMessages.Failed);
            }
        }
        /// <summary>
        /// Retrieves product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        [HttpGet("GetProductByProductId/{id}")]
        public async Task<IActionResult> GetProductByProductIdAsync([FromRoute] int id)
        {
            try
            {
                Product? product = await _productsService.GetProductByProductId(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(new { data = product, message = RetrieveProductsMessages.Success});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while retrieving the product : {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, RetrieveProductsMessages.Failed);
            }
        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProductByNameAsync([FromQuery] string productName)
        {
            try
            {
                List<Product>? products = await _productsService.SearchProductByName(productName);
                if(products == null)
                {
                    return NotFound();
                }
                return Ok(new
                {
                    data = products,
                    message = RetrieveProductsMessages.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for products.");
                return StatusCode(StatusCodes.Status500InternalServerError, RetrieveProductsMessages.Failed);
            }
        }
        [HttpGet("GetProductByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetProductByCategoryIdAsync([FromRoute] int categoryId)
        {
            try
            {
                List<Product>? products = await _productsService.GetProductsByCategoryId(categoryId);
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found for category ID {categoryId}.");
                }
                return Ok(new
                {
                    data = products,
                    message = RetrieveProductsMessages.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving products for category ID {categoryId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, RetrieveProductsMessages.Failed);
            }
        }
    }
}
