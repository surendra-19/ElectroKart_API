using ElectroKart.Common.Data;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ElectroKart.DataAccess
{
    public class AuthDataAccess
    {
        private readonly IConfiguration _configuration;
        public AuthDataAccess(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public async Task<LoginResult> LoginUser(LoginDTO login)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            using var command = new SqlCommand("usp_Customer_Login", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", login.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", login.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Password", login.Password);

            var status = new SqlParameter("@Status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(status);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            Customer? customer = null;

            if (await reader.ReadAsync())
            {
                customer = new Customer
                {
                    Cust_Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                    FirstName = reader.GetString(reader.GetOrdinal("CustomerName")).Split(" ")[0],
                    LastName = reader.GetString(reader.GetOrdinal("CustomerName")).Split(" ").ElementAtOrDefault(1) ?? "",
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.GetString(reader.GetOrdinal("ContactNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                };
            }

            await reader.CloseAsync();

            int statusparam = (int)(status.Value ?? 0);

            return new LoginResult
            {
                Status = statusparam,
                Customer = statusparam == 1 ? customer : null
            };
        }
    }
}
