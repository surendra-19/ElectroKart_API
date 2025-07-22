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
        private readonly ProductsService _productsService;
        public ProductsController(ProductsService productService)
        {
            _productsService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = await _productsService.GetAllProducts();
                if (products == null || products.Count == 0)
                {
                    return NotFound("No products found.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                Product? product = await _productsService.GetProductById(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the product.");
            }
        }
    }
}
