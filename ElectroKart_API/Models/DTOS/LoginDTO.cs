﻿namespace ElectroKart_API.Models.DTOS
{
    public class LoginDTO
    {
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string Password { get; set; } = "";
    }
}
