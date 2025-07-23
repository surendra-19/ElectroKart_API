using ElectroKart.Common.DTOS;
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
    public class CustomerDataAccess
    {
        private readonly IConfiguration _configuration;

        public CustomerDataAccess(IConfiguration config)
        {
            _configuration = config;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
        }

        public async Task<int> UpdateCustomerDetails(CustomerUpdateDTO customer, int Cust_Id)
        {
            using var connection = GetConnection();
            string query = @"UPDATE Customers
                             SET FirstName = @FirstName,
                                 LastName = @LastName,
                                 Email = @Email,
                                 Phone = @Phone,
                                 Address = @Address
                             WHERE Cust_Id = @Cust_Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@Phone", customer.Phone);
            command.Parameters.AddWithValue("@Address", customer.Address);
            command.Parameters.AddWithValue("@Cust_Id", Cust_Id);
            await connection.OpenAsync();
            int result = await command.ExecuteNonQueryAsync();
            return result == 1 ? 1 : 0;
        }
    }
}
