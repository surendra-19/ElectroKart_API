using ElectroKart.Common.DTOS;
using ElectroKart.Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElectroKart.Service;

namespace ElectroKart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly AuthorizationService _authService;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(CustomerService customerService, AuthorizationService authService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _authService = authService;
            _logger = logger;
        }
        /// <summary>
        /// Updates the details of an existing customer.
        /// </summary>
        /// <param name="customer">The updated customer information.</param>
        /// <param name="Cust_Id">The ID of the customer to update.</param>
        /// <returns>
        /// 200 OK if the update is successful,  
        /// 409 Conflict if the email or phone number is already registered to another customer,  
        /// 500 Internal Server Error if the update fails.
        /// </returns>
        [HttpPut("UpdateCustomerDetails")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync([FromBody] CustomerUpdateDTO customer, int Cust_Id)
        {
            try
            {
                var EmailCheck = await _authService.IsEmailRegistered(customer.Email, Cust_Id);
                if (EmailCheck)
                {
                    return Conflict(SignUpMessages.EmailAlreadyRegistered);
                }
                var PhoneCheck = await _authService.IsPhoneRegistered(customer.Phone, Cust_Id);
                if (PhoneCheck)
                {
                    return Conflict(SignUpMessages.PhoneAlreadyExists);
                }
                var result = await _customerService.UpdateCustomerDetails(customer, Cust_Id);
                if (result != 1)
                {
                    return StatusCode(500, UpdateCustomerMessages.ServerError);
                }
                return Ok(UpdateCustomerMessages.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer details for Customer ID: {Cust_Id}", Cust_Id);
                return StatusCode(500, UpdateCustomerMessages.ServerError);
            }

        }
    }
}
