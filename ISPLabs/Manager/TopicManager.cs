using ISPLabs.Models;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class TopicManager
    {
        private OracleConnection _conn;
        private string _lastError;

        public string LastError { get { return _lastError; } }

        public TopicManager(OracleConnection conn) => _conn = conn; 

        public async Task<bool> CreateAsync(Topic topic, ForumMessage initialMessage)
        {
            var cmd = OracleHelper.SetupProcCmd("insert_topic", _conn, false);
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
            await cmd.ExecuteNonQueryAsync();
            if (OracleHelper.BoolResultWithError(cmd, out _lastError))
            {
                topic.Id = Int32.Parse(cmd.Parameters["new_topic_id"].Value.ToString());
                topic.Date = ((OracleDate)cmd.Parameters["topic_date"].Value).Value;
                initialMessage.Id = Int32.Parse(cmd.Parameters["new_message_id"].Value.ToString());
                initialMessage.TopicId = topic.Id;
                initialMessage.Date = topic.Date;
                topic.Messages.Add(initialMessage);
                return true;
            }
            return false;
        }

        public async Task<Topic> GetByIdAsync(int id)
        {
            var cmd = OracleHelper.SetupProcCmd("get_topic_eager", _conn, false);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("topic_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("topic_is_closed", OracleDbType.Int32, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("result_messages", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            var topic = new Topic { Id = id };
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (cmd.Parameters["topic_name"].Value == DBNull.Value)
                    return null;
                topic.Name = cmd.Parameters["topic_name"].Value.ToString();
                topic.IsClosed = cmd.Parameters["topic_is_closed"].Value.ToString() == "1";
                while(await reader.ReadAsync())
                {
                    var msg = ForumMessageManager.Convert(reader);
                    msg.User = new User { Login = reader["user_login"].ToString() };
                    msg.User.Role = RoleManager.Convert(reader);
                    msg.User.RoleId = msg.User.Role.Id;
                    topic.Messages.Add(msg);
                }
            }
            return topic;
        }

        public async Task<Topic> GetByIdWithUserAsync(int id)
        {
            var cmd = OracleHelper.SetupProcCmd("get_topic_with_user", _conn, false);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("category_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            TopicManager.AppendOutPars(cmd);
            UserManager.AppendOutPars(cmd);
            RoleManager.AppendOutPars(cmd);
            await cmd.ExecuteNonQueryAsync();
            var topic = TopicManager.Convert(cmd.Parameters);
            topic.User = UserManager.Convert(cmd.Parameters);
            topic.UserId = topic.User.Id;
            topic.User.Role = RoleManager.Convert(cmd.Parameters);
            topic.User.RoleId = topic.User.Role.Id;
            return topic;
        }

        public async Task<bool> UpdateAsync(Topic topic)
        {
            var cmd = OracleHelper.SetupProcCmd("update_topic", _conn);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = topic.Id;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2, 255).Value = topic.Name;
            cmd.Parameters.Add("p_is_closed", OracleDbType.Int32).Value = topic.IsClosed ? 1 : 0;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResult(cmd);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cmd = OracleHelper.SetupProcCmd("delete_topic", _conn);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
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

        public static Topic Convert(OracleParameterCollection pars)
        {
            var topic = new Topic();
            topic.Id = Int32.Parse(pars["topic_id"].Value.ToString());
            topic.Name = pars["topic_name"].Value.ToString();
            topic.Date = ((OracleDate)pars["topic_date"].Value).Value;
            topic.CategoryId = Int32.Parse(pars["category_id"].Value.ToString());
            topic.IsClosed = pars["topic_is_closed"].Value.ToString() == "1";
            return topic;
        }

        public static void AppendOutPars(OracleCommand cmd)
        {
            cmd.Parameters.Add("topic_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("topic_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("topic_date", OracleDbType.Date).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("topic_is_closed", OracleDbType.Int32).Direction = ParameterDirection.Output;
        }
    }
}
