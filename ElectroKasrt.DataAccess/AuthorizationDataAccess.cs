using ElectroKart.Common.Data;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ElectroKart.DataAccess
{
    public class AuthorizationDataAccess
    {
        private readonly IConfiguration _configuration;
        public AuthorizationDataAccess(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
        }
        public async Task<LoginResult> LoginUser(LoginDTO login)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_Customer_Login", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@UserIdentifier", login.UserIdentifier);
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
        public async Task<bool> IsEmailRegistered(string email, int? Cust_Id)
        {
            using var connection = GetConnection();
            string query = (Cust_Id == null)
                ? "SELECT 1 FROM Customers WHERE Email = @Email"
                : "SELECT 1 FROM Customers WHERE Email = @Email AND Cust_Id != @Cust_Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email",email);
            if (Cust_Id != null)
            {
                command.Parameters.AddWithValue("@Cust_Id", Cust_Id);
            }
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
        public async Task<bool> IsPhoneRegistered(string phone,int? Cust_Id)
        {
            using var connection = GetConnection();
            string query = (Cust_Id == null)
                ? "SELECT 1 FROM Customers WHERE Phone = @Phone"
                : "SELECT 1 FROM Customers WHERE Phone = @Phone AND Cust_Id != @Cust_Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone",phone);
            if (Cust_Id != null)
            {
                command.Parameters.AddWithValue("@Cust_Id",Cust_Id);
            }
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
        public async Task<int> RegisterUser(SignUpDTO signUp)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_Customer_Registration", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@FirstName",signUp.FirstName);
            command.Parameters.AddWithValue("@LastName", signUp.LastName);
            command.Parameters.AddWithValue("@Email", signUp.Email);
            command.Parameters.AddWithValue("@Phone", signUp.Phone);
            command.Parameters.AddWithValue("@Address", signUp.Address);
            command.Parameters.AddWithValue("@Password", signUp.Password);

            await connection.OpenAsync();

            int result = await command.ExecuteNonQueryAsync();
            return result > 0 ? 1 : 0; // 1 means success, 0 means failure
        }
    }
}
