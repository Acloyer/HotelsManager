using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;

namespace HotelsManager.Models.HotelsRepository
{
    public interface IHotelRepository
    {
        void Create(Hotel Hotel);
        void Delete(int id);
        Hotel Get(int id);
        List<Hotel> GetHotels();
        void Update(Hotel Hotel);
        void CreateOrder(Order order);
    }


    public class HotelRepository : IHotelRepository
    {
        string connectionString = @"Data Source=LAPTOP-8U7UGFTE;Initial Catalog=HotelsDB;
								Integrated Security=SSPI;TrustServerCertificate=True;";
        public HotelRepository(string conn)
        {
            connectionString = conn;
        }
        public List<Hotel> GetHotels()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Hotel>("SELECT * FROM Hotels").ToList();
            }
        }

        public Hotel Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Hotel>("SELECT * FROM Hotels WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Create(Hotel Hotel)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (Hotel.Price != null && Hotel.Name != null)
                {
                    var sqlQuery = "INSERT INTO Hotels (Name, Price, Currency, Stars) VALUES(@Name, @Price, @Currency, @Stars)";
                    db.Execute(sqlQuery, Hotel);
                }
            }
        }

        public void Update(Hotel Hotel)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (Hotel.Price != null && Hotel.Name != null && Hotel.Currency != null)
                {
                    var sqlQuery = "UPDATE Hotels SET Name = @Name, Price = @Price, Currency = @Currency, Stars = @Stars WHERE Id = @Id";
                    db.Execute(sqlQuery, Hotel);
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Hotels WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        // ORDERS MOMENTG
        // ORDERS MOMENTG
        // ORDERS MOMENTG

        public void CreateOrder(Order order)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (order != null)
                {
                    if (order.UserName != null && order.HotelId != null && order.HotelName != null && order.HotelPrice != null)
                    {
                        var sqlQuery = "INSERT INTO Orders (UserName, HotelId, HotelName, HotelStars, HotelPrice, HotelCurrency) VALUES(@UserName, @HotelId, @HotelName, @HotelStars, @HotelPrice, @HotelCurrency)";
                        db.Execute(sqlQuery, order);
                    }
                }
            }
        }
    }
}