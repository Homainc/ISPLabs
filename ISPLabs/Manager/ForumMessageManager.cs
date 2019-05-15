using System;
using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Types;
using System.Threading.Tasks;
using ISPLabs.Services;

namespace ISPLabs.Manager
{
    public class ForumMessageManager
    {
        private OracleConnection _conn;

        public ForumMessageManager(OracleConnection conn) => _conn = conn;

        public bool Create(ForumMessage message, out string error, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = new OracleCommand("insert_message", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("pass_text", OracleDbType.Varchar2, 255).Value = message.Text;
            cmd.Parameters.Add("pass_topic_id", OracleDbType.Int32).Value = message.TopicId;
            cmd.Parameters.Add("pass_user_id", OracleDbType.Int32).Value = message.UserId;
            cmd.Parameters.Add("message_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("message_date", OracleDbType.Date).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_email", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_login", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_reg_date", OracleDbType.Date).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("role_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("role_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
	        cmd.Parameters.Add("err", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            if(cmd.Parameters["result"].Value.ToString() == "1")
            {
                error = "";
                message.Id = Int32.Parse(cmd.Parameters["message_id"].Value.ToString());
                message.Date = ((OracleDate)cmd.Parameters["message_date"].Value).Value;
                message.User = UserManager.Convert(cmd.Parameters);
                message.User.Role = RoleManager.Convert(cmd.Parameters);
                message.User.RoleId = message.User.Role.Id;
                return true;
            }
            error = cmd.Parameters["err"].Value.ToString();
            return false;
        }

        public async Task<bool> UpdateAsync(ForumMessage msg, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = OracleHelper.SetupProcCmd("update_message", _conn);
            cmd.Parameters.Add("pass_id", OracleDbType.Int32).Value = msg.Id;
            cmd.Parameters.Add("pass_text", OracleDbType.Varchar2, 255).Value = msg.Text;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResult(cmd);
        }

        public async Task<bool> DeleteAsync(int id, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = new OracleCommand("delete_message", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("pass_id", OracleDbType.Int32).Value = id;
            await cmd.ExecuteNonQueryAsync();
            return cmd.Parameters["result"].Value.ToString() == "1";
        }

        public static ForumMessage Convert(DbDataReader reader)
        {
            var msg = new ForumMessage();
            msg.Id = Int32.Parse(reader["message_id"].ToString());
            msg.Text = reader["message_text"].ToString();
            msg.TopicId = Int32.Parse(reader["topic_id"].ToString());
            msg.UserId = Int32.Parse(reader["user_id"].ToString());
            msg.Date = (DateTime)reader["message_date"];
            return msg;
        }
    }
}
