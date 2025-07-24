using ElectroKart.Common.DTOS;
using ElectroKart.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Service
{
    public class OrdersService
    {
        private readonly OrdersDataAccess _OrderDataAccess;
        public OrdersService(OrdersDataAccess ordersDataAccess) 
        {
            _OrderDataAccess = ordersDataAccess;
        }
        public async Task<int> CreateCustomerOrder(PlaceOrderDTO placeOrder)
        {
            return await _OrderDataAccess.CreateCustomerOrder(placeOrder);
        }
    }
}
