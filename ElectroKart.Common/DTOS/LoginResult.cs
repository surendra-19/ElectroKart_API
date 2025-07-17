using ElectroKart.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.DTOS
{
    public class LoginResult
    {
        public int Status { get; set; }
        public Customer? Customer { get; set; }
    }
}
