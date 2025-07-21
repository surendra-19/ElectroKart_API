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
        private readonly AuthService _authService;
        public CustomerController(CustomerService customerService, AuthService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync([FromBody] CustomerUpdateDTO customer, int Cust_Id)
        {
            var EmailCheck = await _authService.IsEmailRegistered(customer.Email);
            if (EmailCheck)
            {
                return Conflict(SignUpMessages.EmailAlreadyRegistered);
            }
            var PhoneCheck = await _authService.IsPhoneRegistered(customer.Phone);
            if (PhoneCheck)
            {
                return Conflict(SignUpMessages.PhoneAlreadyExists);
            }
            var result = await _customerService.UpdateCustomerDetails(customer, Cust_Id);
            return result == 1 ? Ok(UpdateCustomerMessages.Success) : StatusCode(500, UpdateCustomerMessages.ServerError);

        }
    }
}
