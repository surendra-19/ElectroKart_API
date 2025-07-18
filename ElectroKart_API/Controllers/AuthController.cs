using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Data;
using ElectroKart.Common.Messages;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO logindto)
        {
            try
            {
                var result = await _authService.LoginUser(logindto);

                if (result.Status == 1)
                {
                    return Ok(new { result.Customer, Message = LoginMessages.LoginSuccess });
                }
                else if (result.Status == 2)
                {
                    return Unauthorized(LoginMessages.IncorrectPassword);
                }
                else if (result.Status == 3)
                {
                    return NotFound(LoginMessages.UserNotFound);
                }
                else
                {
                    return StatusCode(500, LoginMessages.ServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in for user: {User}", logindto.UserIdentifier);
                return StatusCode(500, LoginMessages.ServerError);
            }
        }
    }
}
