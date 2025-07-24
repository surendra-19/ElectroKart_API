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
    public class OrdersDataAccess
    {
        private readonly IConfiguration _configuration;
        public OrdersDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
        }
        public async Task<int> CreateCustomerOrder(PlaceOrderDTO placeOrder)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_PlaceCustomerOrder", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Cust_Id", placeOrder.Cust_Id);
            var orderItemsTable = new DataTable();
            orderItemsTable.Columns.Add("ProductId", typeof(int));
            orderItemsTable.Columns.Add("Quantity", typeof(int));
            foreach (var item in placeOrder.OrderItems)
            {
                orderItemsTable.Rows.Add(item.ProductId, item.Quantity);
            }
            var tvpParam = new SqlParameter("@OrderItems", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderItemsType", 
                Value = orderItemsTable
            };
            command.Parameters.Add(tvpParam);
            var status = new SqlParameter("@Status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(status);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return (int)status.Value;
        }
    }
}
