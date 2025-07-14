using ElectroKart.Service;
using ElectroKart_API.Data;
using ElectroKart_API.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectroKart_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly Context _dbContext;
        public AuthController(AuthService authService,Context context)
        {
            _authService = authService;
            _dbContext = context;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO login)
        {
            if(string.IsNullOrEmpty(login.Email) && string.IsNullOrEmpty(login.PhoneNumber))
            {
                return BadRequest("Email or Phone Number is required.");

            }
            if (string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Password is required");
            }
            var user = await _dbContext.Customers.FirstOrDefaultAsync(
                    customer => (customer.Email == login.Email || customer.Phone == login.PhoneNumber)
            );
            if(user == null)
            {
                return NotFound("Customer not found");
            }
            if(user.Password != login.Password)
            {
                return Unauthorized("Invalid password");
            }
            var res = new
            {
                message = "Login successful",
                customer = new
                {
                    user.Cust_Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone
                }
            };
            return Ok(res);
        }
    }
}
