using Dapper;
using ISPLabs.Models;
using ISPLabs.Models.API;
using ISPLabs.Repositories.Interfaces;
using ISPLabs.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ISPLabs.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private string connectionString;
        public TopicRepository(string conn) => connectionString = conn;

        public Topic Append(Topic topic)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("INSERT INTO [dbo].[Topic] ");
                sql.Append("(Name, IsClosed, Category_id, User_id) ");
                sql.Append("VALUES(@Name, @IsClosed, @Category_id, @User_id);");
                var affectedRows = db.Execute(
                    sql.ToString(),
                    new { Name = topic.Name, IsClosed = topic.IsClosed, Category_id = topic.Category.Id, User_id = topic.User.Id });
                if (affectedRows == 0)
                    return null;
                sql.Clear();
                sql.Append("SELECT * FROM [dbo].[Topic] AS t ");
                sql.Append("LEFT JOIN [dbo].[Category] AS c ON t.Category_id = c.Id ");
                sql.Append("LEFT JOIN [dbo].[User] AS u ON t.User_id = u.Id ");
                sql.Append("WHERE t.Category_id = @CategoryId AND t.Name = @Name");
                return db.Query<Topic, Category, User, Topic>(
                    sql.ToString(),
                    (t, category, user) =>
                    {
                        t.Category = category;
                        t.User = user;
                        return t;
                    },
                    new { CategoryId = topic.Category.Id, Name = topic.Name },
                    splitOn: "Id").FirstOrDefault();
            }
        }

        public HashSet<TopicAPIModel> GetAll()
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Topic] AS t ");
                return db.Query<TopicAPIModel>(sql.ToString()).ToHashSet();
            }
        }

        public TopicAPIModel GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Topic] ");
                sql.Append("WHERE Id = @Id;");
                var topic = db.Query<TopicAPIModel>(sql.ToString(), new { Id = id }).FirstOrDefault();
                sql.Clear();
                sql.Append("SELECT u.Id, u.Login, r.Name AS Role, TUser.MessagesCount FROM ( ");
                sql.Append("    SELECT msgUser.Id, COUNT(fm.Id) AS MessagesCount FROM ( ");
                sql.Append("        SELECT u.Id AS Id FROM [dbo].[ForumMessage] AS fm ");
                sql.Append("        LEFT JOIN [dbo].[User] AS u ON fm.User_id = u.Id ");
                sql.Append("        WHERE fm.Topic_id = @Id ");
                sql.Append("    ) AS msgUser ");
                sql.Append("    LEFT JOIN [dbo].[ForumMessage] AS fm ON fm.User_id = msgUser.Id ");
                sql.Append("    GROUP BY msgUser.Id ");
                sql.Append(") AS TUser ");
                sql.Append("LEFT JOIN [dbo].[User] AS u ON u.Id = TUser.Id ");
                sql.Append("LEFT JOIN [dbo].[Role] AS r ON r.Id = u.Role_id;");
                var usersDictionary = db.Query<ForumUserAPIModel>(sql.ToString(), new { Id = id }).ToDictionary(x => x.Id);
                sql.Clear();
                sql.Append("SELECT * FROM [dbo].[ForumMessage] AS fm ");
                sql.Append("LEFT JOIN [dbo].[User] AS u ON u.Id = fm.User_id ");
                sql.Append("WHERE Topic_id = @Id;");
                var messages = db.Query<MessageAPIModel, ForumUserAPIModel, MessageAPIModel>(
                    sql.ToString(),
                    (msg, user) =>
                    {
                        msg.User = usersDictionary[user.Id];
                        return msg;
                    },
                    new { Id = id },
                    splitOn: "Id").ToHashSet();
                topic.Messages = messages;
                return topic;
            }
        }

        public Topic GetByIdWithCategory(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Topic] AS t ");
                sql.Append("LEFT JOIN [dbo].[Category] AS c ON t.Category_id = c.Id ");
                sql.Append("LEFT JOIN [dbo].[User] AS u ON t.User_id = u.Id ");
                sql.Append("WHERE t.Id = @Id");
                return db.Query<Topic, Category, User, Topic>(
                    sql.ToString(),
                    (topic, category, user) =>
                    {
                        topic.Category = category;
                        topic.User = user;
                        return topic;
                    },
                    new { Id = id },
                    splitOn: "Id").FirstOrDefault();
            }
        }

        public Topic GetByIdWithUser(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Topic] AS t ");
                sql.Append("LEFT JOIN [dbo].[User] AS u ON t.User_id = u.Id ");
                sql.Append("WHERE t.Id = @Id;");
                return db.Query<Topic, User, Topic>(
                    sql.ToString(),
                    (topic, user) => {
                        topic.User = user;
                        return topic;
                    },
                    new { Id = id },
                    splitOn: "Id").FirstOrDefault();
            }
        }

        public bool Remove(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("DELETE FROM [dbo].[ForumMessage] ");
                sql.Append("WHERE Topic_id = @Id");
                db.Execute(sql.ToString(), new { Id = id });
                sql.Clear();
                sql.Append("DELETE FROM [dbo].[Topic] ");
                sql.Append("WHERE Id = @Id");
                return db.Execute(sql.ToString(), new { Id = id }) == 1;
            }
        }

        public bool Update(TopicAPIModel topic)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("UPDATE [dbo].[Topic] ");
                sql.Append("SET Name = @Name, IsClosed = @IsClosed ");
                sql.Append("WHERE Id = @Id;");
                return db.Execute(sql.ToString(), topic) == 1;
            }
        }
    }
}
