﻿using ElectroKart.Common.Models;
using ElectroKart.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Service
{
    public class ProductsService
    {
        private readonly ProductsDataAccess _productsDataAccess;
        public ProductsService(ProductsDataAccess productsDataAccess)
        {
            _productsDataAccess = productsDataAccess;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _productsDataAccess.GetAllProducts();
        }
        public async Task<Product?> GetProductByProductId(int id)
        {
            return await _productsDataAccess.GetProductByProductId(id);
        }
        public async Task<List<Product>?> SearchProductByName(string productName)
        {
            return await _productsDataAccess.SearchProductByName(productName);
        }
        public async Task<List<Product>?> GetProductsByCategoryId(int categoryId)
        {
            return await _productsDataAccess.GetProductsByCategoryId(categoryId);
        }
    }
}
