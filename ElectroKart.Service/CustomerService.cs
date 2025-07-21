using ElectroKart.Common.DTOS;
using ElectroKart.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Service
{
    public class CustomerService
    {
        private readonly CustomerDataAccess _customerDataAccess;
        public CustomerService(CustomerDataAccess customerDataAccess)
        {
            _customerDataAccess = customerDataAccess;
        }
        public async Task<int> UpdateCustomerDetails(CustomerUpdateDTO customer, int Cust_Id)
        {
            return await _customerDataAccess.UpdateCustomerDetails(customer, Cust_Id);
        }
    }
}
