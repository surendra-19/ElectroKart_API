using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.Messages
{
    public class OrderProductMessages
    {
        public const string OrderSuccess = "Order placed successfully.";
        public const string OrderError = "An error occurred while placing the order.";
        public const string OutOfStock = "One or more product(s) are out of stock.";
        public const string EmptyCart = "No products were selected for the order.";
    }
}
