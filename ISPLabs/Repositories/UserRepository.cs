using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using ISPLabs.Models;
using ISPLabs.Models.API;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Repositories
{
    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string connectStr) => connectionString = connectStr;

        public User Append(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var str = new StringBuilder();
                str.Append("INSERT INTO [dbo].[User] (Login, Email, Password, RegistrationDate, Role_ID) ");
                str.Append("VALUES (@Login, @Email, @Password, @RegistrationDate, @Role_id);");
                db.Execute(
                    str.ToString(),
                    new {
                        user.Login,
                        user.Email,
                        user.Password,
                        user.RegistrationDate,
                        Role_id = 1
                    }
                );
                return Login(user.Email, user.Password);
            }
        }

        public UserAPIModel GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[User] AS u ");
                sql.Append("LEFT JOIN [dbo].[Role] AS r ON u.Role_id = r.Id ");
                sql.Append("WHERE u.Id = @Id;");
                return db.Query<UserAPIModel, RoleAPIModel, UserAPIModel>(
                    sql.ToString(),
                    (user, role) => {
                        user.Role = role.Name;
                        return user;
                    },
                    new { @Id = id },
                    splitOn: "Id").FirstOrDefault();
            }
        }

        public HashSet<UserAPIModel> GetAll()
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[User] AS u ");
                sql.Append("LEFT JOIN [dbo].[Role] AS r ON r.Id = u.Role_id ");
                return db.Query<UserAPIModel, RoleAPIModel, UserAPIModel>(
                    sql.ToString(),
                    (user, role) => {
                        user.Role = role.Name;
                        return user;
                    },
                    splitOn:"Id").ToHashSet();
            }
        }

        public User GetByEmail(string email)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[User] ");
                sql.Append("WHERE Email = @Email;");
                return db.Query<User>(sql.ToString(), new { Email = email }).FirstOrDefault();
            }
        }

        public int GetMessagesCount(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT COUNT(fm.Id) FROM [dbo].[ForumMessage] AS fm ");
                sql.Append("WHERE fm.User_id = @Id ");
                sql.Append("GROUP BY fm.User_id;");
                return db.Query<int>(sql.ToString(), new { Id = id }).FirstOrDefault();
            }
        }

        public User Login(string email, string password)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var str = new StringBuilder();
                str.Append("SELECT * FROM [dbo].[User] AS u ");
                str.Append("LEFT JOIN [dbo].[Role] AS r ON u.Role_id = r.id ");
                str.Append("WHERE u.Email = @Email AND u.Password = @Password;");
                return db.Query<User, Role, User>(
                    str.ToString(),
                    (user, role) => 
                    {
                        user.Role = role;
                        return user;
                    },
                    new { Email = email, Password = password },
                    splitOn:"Role_id").FirstOrDefault();
            }
        }

        public bool Update(User user)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("UPDATE [dbo].[User] ");
                sql.Append("SET Login = @Login, Role_id = @Role_id, Email = @Email, Password = @Password ");
                sql.Append("WHERE Id = @Id;");
                return db.Execute(sql.ToString(),
                    new { user.Id, user.Login, user.Email, user.Password, Role_id = user.Role.Id }) == 1;
            }
        }
    }
}
