using Dapper;
using ISPLabs.Models;
using ISPLabs.Models.API;
using ISPLabs.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ISPLabs.Repositories
{
    public class ForumMessageRepository : IForumMessageRepository
    {
        private string connectionString;
        public ForumMessageRepository(string conn) => connectionString = conn;

        public ForumMessage Append(ForumMessage message)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("INSERT INTO [dbo].[ForumMessage] ");
                sql.Append("(Text, Date, Topic_id, User_id) ");
                sql.Append("VALUES(@Text, @Date, @Topic_id, @User_id);");
                var affectedRows = db.Execute(
                    sql.ToString(),
                    new { Text = message.Text, Date = message.Date, Topic_id = message.Topic.Id, User_id = message.User.Id });
                if (affectedRows == 0)
                    return null;
                sql.Clear();
                sql.Append("SELECT * FROM [dbo].[ForumMessage] AS fm ");
                sql.Append("WHERE fm.Text = @Text AND fm.User_id = @User_id AND fm.Topic_id = @Topic_id;");
                var msg = db.Query<ForumMessage>(
                    sql.ToString(),
                    new { Text = message.Text, User_id = message.User.Id, Topic_id = message.Topic.Id }
                    ).FirstOrDefault();
                msg.User = message.User;
                msg.Topic = message.Topic;
                return msg;
            }
        }

        public ISet<MessageAPIModel> GetAllInTopic(int topicId)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(MessageAPIModel message)
        {
            throw new NotImplementedException();
        }
    }
}
