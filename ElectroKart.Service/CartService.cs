using ElectroKart.Common.DTOS;
using ElectroKart.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Service
{
    public class CartService
    {
        private readonly CartDataAccess _categoryDataAccess;
        public CartService(CartDataAccess categoryDataAccess) 
        {
            _categoryDataAccess = categoryDataAccess;
        }
        public async Task<int> GetCartItems(int customerId)
        {
            return await _categoryDataAccess.GetCartItems(customerId);
        }
        public async Task<int> RemoveFromCart(int itemId,int customerId)
        {
            return await _categoryDataAccess.RemoveFromCart(itemId:itemId, customerId:customerId);
        }
        public async Task<bool> ClearCart(int customerId)
        {
            return await _categoryDataAccess.ClearCart(customerId);
        }
        public async Task<bool> AddToCart(CartItemDTO cartItemDTO)
        {
            return await _categoryDataAccess.AddToCart(cartItemDTO);
        }
    }
}
