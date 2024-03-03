using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using HotelsManager.Models.Users;
using HotelsManager.Models.Orders;

namespace HotelsManager.Models.Repository
{
    public interface IUserRepository
    {
        void Create(User user);
        void Delete(int id);
        User Get(int id);
		User GetPassword(int id);
		User GetId(string Name);
        int GetOrdersCount(string UserName);
        List<User> GetUsers();
        void Update(User user);
    }

    public class UserRepository : IUserRepository
	{
		string connectionString = @"Data Source=LAPTOP-8U7UGFTE;Initial Catalog=HotelsDB;
									Integrated Security=SSPI;TrustServerCertificate=True;";
		public UserRepository(string conn)
		{
			connectionString = conn;
		}
        public List<User> GetUsers()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public User Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public User GetPassword(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT Password FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public User GetId(string Name)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT Id FROM Users WHERE Name = @Name", new { Name }).FirstOrDefault();
            }
        }
        
        public int GetOrdersCount(string UserName)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return int.Parse(db.Query<Order>("SELECT COUNT(*) AS [HotelName] FROM Orders WHERE UserName = @UserName", new { UserName }).FirstOrDefault().HotelName);
            }
        }

        public void Create(User user)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				if (user.Password != null && user.Password.Length >= 6 && user.Password.Length < 40)
				{
					if (user.Name != null && user.Name.Length >= 4 && user.Name.Length < 20)
					{
						var sqlQuery = "INSERT INTO Users (Name, Password) VALUES(@Name, @Password)";
						db.Execute(sqlQuery, user);
					}
				}
                // если мы хотим получить id добавленного пользователя
                //var sqlQuery = "INSERT INTO Users (Name, Password) VALUES(@Name, @Password); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                //user.Id = userId.Value;
            }
		}

		public void Update(User user)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				if(user.Password != null && user.Password.Length > 6 && user.Password.Length < 40)
				{
					if (user.Name != null && user.Name.Length > 4 && user.Name.Length < 20)
					{
						var sqlQuery = "UPDATE Users SET Name = @Name, Password = @Password WHERE Id = @Id";
						db.Execute(sqlQuery, user);
					}
				}
			}
		}

		public void Delete(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var sqlQuery = "DELETE FROM Users WHERE Id = @id";
				db.Execute(sqlQuery, new { id });
			}
		}
    }
}