﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.DTOS
{
    public class OrderResponse
    {
        public List<OrderDTO> Orders { get; set; } = new();
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
