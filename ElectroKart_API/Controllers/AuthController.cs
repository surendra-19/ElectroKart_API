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
        [HttpPost("SignUp")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpDTO signUpDTO)
        {
            try
            {
                // Verifying if the email is already registered
                var RegisteredEmailVerification = await _authService.IsEmailRegistered(signUpDTO.Email);
                // Verifying if the phonenumber is already registered
                var RegisteredPhoneVerification = await _authService.IsPhoneRegistered(signUpDTO.Phone);

                if (RegisteredEmailVerification)
                {
                    return Conflict(SignUpMessages.EmailAlreadyRegistered);
                }
                else if (RegisteredPhoneVerification)
                {
                    return Conflict(SignUpMessages.PhoneAlreadyExists);
                }
                var result = await _authService.RegisterUser(signUpDTO);
                return result == 1 ? Ok(SignUpMessages.SignUpSuccess) : StatusCode(500, SignUpMessages.ServerError);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occured while registering the user : {}",signUpDTO.Email);
                return StatusCode(500,SignUpMessages.ServerError);
            }
        }
    }
}
