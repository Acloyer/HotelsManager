using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;

namespace HotelsManager.Models.Repository
{
    public interface IOrderRepository
    {
        void Create(Order order);
        void Update(Order order);
        void Delete(int id);
        void Delete(int id, string username);
        Order Get(int id); 
        List<Order> GetOrders();
        List<Order> GetOrders(string username);
    }

    public class OrderRepository : IOrderRepository
    {
		string connectionString = @"Data Source=LAPTOP-8U7UGFTE;Initial Catalog=HotelsDB;
									Integrated Security=SSPI;TrustServerCertificate=True;";
		public OrderRepository(string conn)
		{
			connectionString = conn;
		}

        public void Update(Order order)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (order.HotelId != null)
                {
                    string query = "SELECT * FROM Hotels WHERE Id = @HotelId";
                    var a = db.Query<Hotel>(query, new {HotelId = order.HotelId}).ToList();
                    var UserName = order.UserName;
                    var HotelId = 0;
                    var HotelName = "";
                    decimal? HotelPrice = 0.0m;
                    var HotelCurrency = "";
                    var HotelStars = 0;
                    foreach(var asa in a)
                    {
                        HotelId = asa.Id;
                        HotelName = asa.Name;
                        HotelPrice = asa.Price;
                        HotelCurrency = asa.Currency;
                        HotelStars = asa.Stars;
                    }
                    var sqlQuery = "UPDATE Orders SET UserName = @UserName, HotelId = @HotelId, HotelName = @HotelName, HotelPrice = @HotelPrice, HotelCurrency = @HotelCurrency, HotelStars = @HotelStars WHERE Id = @Id";
                    db.Execute(sqlQuery, new { UserName = UserName, HotelId = HotelId, HotelName = HotelName, HotelPrice = HotelPrice, HotelCurrency = HotelCurrency, HotelStars = HotelStars, Id = order.Id});

                }
            }
        }

        public List<Order> GetOrders()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Order>("SELECT * FROM Orders").ToList();
            }
        }
        public List<Order> GetOrders(string username)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Orders WHERE UserName = @UserName";
                return db.Query<Order>(query, new { UserName = username }).ToList();
            }
        }

        public Order Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Order>("SELECT * FROM Orders WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

		public void Create(Order order)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				if (order != null)
				{
                    Console.WriteLine(order.HotelStars);
                    var sqlQuery = "INSERT INTO Orders (UserName, HotelId, HotelName, HotelStars, HotelPrice, HotelCurrency) VALUES(@UserName, @HotelId, @HotelName, @HotelStars, @HotelPrice, @HotelCurrency)";
					db.Execute(sqlQuery, order);
				}
			}
		}

		// Для админов
		public void Delete(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var sqlQuery = "DELETE FROM Orders WHERE Id = @id";
				db.Execute(sqlQuery, new { id });
			}
		}

        // для обычных пользователей Которые могут удалять свои orders через "мои заказы"
        public void Delete(int id, string username)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Orders WHERE Id = @id AND UserName = @username";
                db.Execute(sqlQuery, new { id, username });
            }
        }
    }
}