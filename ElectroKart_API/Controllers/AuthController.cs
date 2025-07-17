using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Data;

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
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO logindto)
        {
            try
            {
                //var userstatus = await _authService.LoginUser(logindto);
                //if (userstatus == 1)
                //{
                //    // some logic
                //}
                //else if (userstatus == 2)
                //{
                //    // some logic
                //}
                //else if (userstatus == 3)
                //{
                //    // some logic
                //}
                //else
                //{
                //    // some logic
                //}
                return Ok(await _authService.LoginUser(logindto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
    }
}
