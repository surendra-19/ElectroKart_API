using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.DTOS
{
    public class OrderItemsCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
