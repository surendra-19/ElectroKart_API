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
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationService _authService;
        private readonly ILogger<AuthorizationController> _logger;
        public AuthorizationController(AuthorizationService authService, ILogger<AuthorizationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user using their email/phone and password.
        /// </summary>
        /// <param name="logindto">Login credentials (email/phone and password).</param>
        /// <returns>
        /// 200 OK with customer data if login is successful,  
        /// 401 Unauthorized if password is incorrect,  
        /// 404 Not Found if user is not found,  
        /// 500 Internal Server Error for any other issue.
        /// </returns>
        [HttpPost("loginUser")]
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

        /// <summary>
        /// Registers a new user with email, phone, and password.
        /// </summary>
        /// <param name="signUpDTO">User registration details.</param>
        /// <returns>
        /// 200 OK if registration is successful,  
        /// 409 Conflict if email or phone already exists,  
        /// 500 Internal Server Error for any other issue.
        /// </returns>
        [HttpPost("SignUpUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpDTO signUpDTO)
        {
            try
            {
                // Verifying if the email is already registered
                var RegisteredEmailVerification = await _authService.IsEmailRegistered(email:signUpDTO.Email, Cust_Id:null);
                // Verifying if the phonenumber is already registered
                var RegisteredPhoneVerification = await _authService.IsPhoneRegistered(phone:signUpDTO.Phone, Cust_Id:null);

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
