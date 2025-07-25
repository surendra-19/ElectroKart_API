using ElectroKart.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.DataAccess
{
    public class CategoryDataAccess
    {
        private readonly IConfiguration _configuration; 
        public CategoryDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection() 
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection")); 
        }
        public async Task<List<Category>> GetAllCategories() 
        {
            List<Category> categories = new List<Category>(); 
            using (var connection = GetConnection()) 
            {
                string query = "SELECT * FROM Categories";
                using (var command = new SqlCommand(query, connection)) 
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync()) 
                    {
                        while (reader.Read()) 
                        {
                            var category = new Category 
                            {
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                isActive = reader.GetInt32(reader.GetOrdinal("isActive"))
                            }; 
                            categories.Add(category); 
                        }
                    }
                }
            }
            return categories; 
        }
    }
}
