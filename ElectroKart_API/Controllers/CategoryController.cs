using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, CategoryService categoryService)
        {
            _categoryService = categoryService; _logger = logger;
        }
        /// <summary>
        /// Retrieves all product categories.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of categories,  
        /// 404 Not Found if no categories exist,  
        /// 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
