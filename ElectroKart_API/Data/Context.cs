﻿using ElectroKart_API.Models.DTOS;
using Microsoft.EntityFrameworkCore;
namespace ElectroKart_API.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
