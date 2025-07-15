using ElectroKart.Common.Data;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ElectroKart.DataAccess
{
    public class AuthDataAccess
    {
        private readonly IConfiguration _configuration;
        public AuthDataAccess(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public async Task<Customer?> LoginUser(LoginDTO login)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            using var command = new SqlCommand("usp_Customer_Login", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", login.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber",login.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Password",login.Password);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                if(reader.FieldCount == 1)
                {
                    //some code
                }
                var customer =  new Customer
                {
                    Cust_Id = reader.GetInt32(reader.GetOrdinal("Cust_Id")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                };
                return customer;
            }
            return null;
        } 
    }
}
