using ElectroKart.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.DataAccess
{
    public class ProductsDataAccess
    {
        private readonly IConfiguration _configuration;
        public ProductsDataAccess(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
        }
        public async Task<List<Product>> GetAllProducts()
        {
            using var connection = GetConnection();
            string query = @"SELECT ProductId, ProductName, ProductDescription, Price, Brand, Category_Id, ImageUrl FROM Products";
            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("Brand_Id")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                };
                products.Add(product);
            }
            return products;
        }
        public async Task<Product?> GetProductByProductId(int id)
        {
            using var connection = GetConnection();
            string query = @"SELECT ProductId, ProductName, ProductDescription, Price, Brand, Category_Id, ImageUrl 
                     FROM Products WHERE ProductId = @ProductId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductId", id);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescription")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("Brand_Id")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("Category_Id")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                };
            }

            return null;
        }
        public async Task<List<Product>?> SearchProductByName(string productName)
        {
            using var connection = GetConnection();
            string query = "SELECT ProductName FROM Products WHERE ProductName LIKE '%'+@ProductName+'%'";
            using var command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@ProductName", productName);
            await connection.OpenAsync();
            List<Product> products = new List<Product>();
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName"))
                };
                products.Add(product);
            }
            return products.Count > 0 ? products : null;
        }
        public async Task<List<Product>?> GetProductsByCategoryId(int categoryId)
        {
            using var connection = GetConnection();
            string query = @"SELECT * FROM Products WHERE Category_Id = @CategoryId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescription")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("Brand_Id")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("Category_Id")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                    isActive = reader.GetInt32(reader.GetOrdinal("isActive"))
                };
                products.Add(product);
            }
            return products;
        }
    }
}
