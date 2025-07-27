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
        public CustomerController(CustomerService customerService, AuthorizationService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }
        [HttpPut("UpdateCustomerDetails")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync([FromBody] CustomerUpdateDTO customer, int Cust_Id)
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
            return result == 1 ? Ok(UpdateCustomerMessages.Success) : StatusCode(500, UpdateCustomerMessages.ServerError);
        }
    }
}
