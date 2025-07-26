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
        public async Task<OrderResponse> GetOrdersByCustomerId(int customerId)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("usp_GetOrdersByCustId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Cust_Id", customerId);
            var statusParam = new SqlParameter("@Status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(statusParam);

            var orderDict = new Dictionary<int, OrderDTO>();

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));

                if (!orderDict.ContainsKey(orderId))
                {
                    var order = new OrderDTO
                    {
                        OrderId = orderId,
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        ShippedDate = reader.IsDBNull(reader.GetOrdinal("ShippedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ShippedDate")),
                        DeliveryDate = reader.IsDBNull(reader.GetOrdinal("ReachedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ReachedDate")),
                        TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                        OrderStatus = reader.GetString(reader.GetOrdinal("OrderStatus"))
                    };
                    orderDict[orderId] = order;
                }

                var item = new OrderItemDTO
                {
                    OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("EachItemPrice"))
                };

                orderDict[orderId].OrderItems.Add(item);
            }
            await reader.CloseAsync();

            int status = (int)(statusParam.Value ?? 0);

            return new OrderResponse
            {
                Orders = orderDict.Values.ToList(),
                StatusCode = status,
                Message = status == 2 ? "Orders retrieved successfully." : "No orders found or an error occurred."
            };
        }


    }
}
