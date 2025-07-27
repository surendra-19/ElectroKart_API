using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.DTOS
{
    public class CancelOrderDTO
    {
        public int OrderID { get; set; } 
        public int Status { get; set; }
    }
}
