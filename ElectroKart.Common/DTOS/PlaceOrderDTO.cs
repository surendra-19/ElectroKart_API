using ElectroKart.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.DTOS
{
    public class PlaceOrderDTO
    {
        public int Cust_Id { get; set; }
        public List<OrderItemsCreateDTO> OrderItems { get; set; } = new List<OrderItemsCreateDTO>();
    }
}
