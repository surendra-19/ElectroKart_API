using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Cust_Id", customerId);
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        public async Task<int> RemoveFromCart(int itemId, int customerId)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_RemoveCartItem", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Item_Id", itemId);
            command.Parameters.AddWithValue("@Cust_Id", customerId);
            var status = new SqlParameter("@Status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(status);
            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            int result = (int)(status.Value ?? 0);
            return result;
        }
        public async Task<bool> ClearCart(int customerId)
        {
            using var connection = GetConnection();
            string query = "DELETE FROM CartItems WHERE Cust_Id = @Cust_Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Cust_Id", customerId);
            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> AddToCart(CartItemDTO cartItemDTO)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_AddCartItem", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Cust_Id", cartItemDTO.CustomerId);
            command.Parameters.AddWithValue("@ProductId", cartItemDTO.ProductId);
            await connection.OpenAsync();
            int result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
