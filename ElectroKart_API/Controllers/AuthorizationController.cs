using ElectroKart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Data;
using ElectroKart.Common.Messages;
using ElectroKart.Common.JwtConfiguration;
using Microsoft.AspNetCore.Authorization;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationService _authService;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly JwtTokenService _jwtTokenService;
        public AuthorizationController(AuthorizationService authService, ILogger<AuthorizationController> logger,JwtTokenService jwtTokenService)
        {
            _authService = authService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
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
                    var token = _jwtTokenService.GenerateToken(result.Customer!);
                    return Ok(new { result.Customer,Token = token, Message = LoginMessages.LoginSuccess });
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
                else if (string.IsNullOrEmpty(signUpDTO.Password) || signUpDTO.Password.Length < 6)
                {
                    return BadRequest("Password must be at least 6 characters long.");
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
        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPasswordAsync([FromBody] string email)
        //{
        //    try
        //    {
        //        return Ok("Password reset link has been sent to your email.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while processing forgot password for email: {Email}", email);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}
        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO resetPasswordDTO)
        //{
        //    try
        //    {
        //        return Ok("Password has been reset successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while resetting password for email: {Email}", resetPasswordDTO.Email);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}
    }
}
