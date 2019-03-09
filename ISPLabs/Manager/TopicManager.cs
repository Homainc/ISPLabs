using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class TopicManager
    {
        private OracleConnection _conn;

        public TopicManager(OracleConnection conn) => _conn = conn; 

        public bool Create(Topic topic, ForumMessage initialMessage, out string error)
        {
            var cmd = new OracleCommand("insert_topic", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2, 255).Value = topic.Name;
            cmd.Parameters.Add("p_category_id", OracleDbType.Int32).Value = topic.CategoryId;
            cmd.Parameters.Add("p_user_id", OracleDbType.Int32).Value = topic.User.Id;
            cmd.Parameters.Add("p_is_closed", OracleDbType.Int32).Value = topic.IsClosed ? 1 : 0;
            cmd.Parameters.Add("p_message_text", OracleDbType.Varchar2, 255).Value = initialMessage.Text;
            cmd.Parameters.Add("new_topic_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("new_message_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("topic_date", OracleDbType.Date).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            if (cmd.Parameters["result"].Value.ToString() == "1")
            {
                topic.Id = Int32.Parse(cmd.Parameters["new_topic_id"].Value.ToString());
                topic.Date = ((OracleDate)cmd.Parameters["topic_date"].Value).Value;
                initialMessage.Id = Int32.Parse(cmd.Parameters["new_message_id"].Value.ToString());
                initialMessage.TopicId = topic.Id;
                initialMessage.Date = topic.Date;
                topic.Messages.Add(initialMessage);
                error = "";
                return true;
            }
            error = cmd.Parameters["er"].Value.ToString();
            return false;
        }

        public async Task<Topic> GetByIdAsync(int id)
        {
            var cmd = new OracleCommand("get_topic_eager", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("topic_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("result_messages", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            var topic = new Topic { Id = id };
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (cmd.Parameters["topic_name"].Value == DBNull.Value)
                    return null;
                topic.Name = cmd.Parameters["topic_name"].Value.ToString();
                while(await reader.ReadAsync())
                {
                    var msg = ForumMessageManager.Convert(reader);
                    msg.User = new User { Login = reader["user_login"].ToString() };
                    topic.Messages.Add(msg);
                }
            }
            return topic;
        }

        public static Topic Convert(DbDataReader reader)
        {
            var topic = new Topic();
            topic.Id = Int32.Parse(reader["topic_id"].ToString());
            topic.Name = reader["topic_name"].ToString();
            topic.Date = (DateTime)reader["topic_date"];
            topic.CategoryId = Int32.Parse(reader["category_id"].ToString());
            topic.IsClosed = reader["topic_is_closed"].ToString() == "1";
            return topic;
        }
    }
}
