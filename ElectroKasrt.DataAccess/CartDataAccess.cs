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
    public class CartDataAccess
    {
        private readonly IConfiguration _configuration;
        public CartDataAccess(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
        }
        public async Task<int> GetCartItems(int customerId)
        {
            using var connection = GetConnection();
            string query = "SELECT COUNT(*) FROM CartItems WHERE Cust_Id = @Cust_Id";
            using var command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@Cust_Id", customerId);
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task<bool> RemoveFromCart(int itemId,int customerId)
        {
            using var connection = GetConnection();
            string query = "DELETE FROM CartItems WHERE Item_Id = @Item_Id AND Cust_Id = @Cust_Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Item_Id", itemId);
            command.Parameters.AddWithValue("@Cust_Id", customerId);
            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
