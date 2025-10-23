using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;

namespace Class_3_12.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ShipAddress { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class NorthwndDb
    {
        private readonly string _connectionString;
        public NorthwndDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Order> GetOrders()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT OrderID, ShipAddress, OrderDate FROM Orders";
            connection.Open();

            var reader = cmd.ExecuteReader();
            var orders = new List<Order>();
            while(reader.Read())
            {
                orders.Add(new Order
                {
                    Id = (int)reader["OrderID"],
                    ShipAddress = (string)reader["ShipAddress"],
                    OrderDate = (DateTime)reader["OrderDate"]
                });
            }

            return orders;
        }
    }
}
