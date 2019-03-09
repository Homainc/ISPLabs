using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ISPLabs.Manager
{
    public class ForumMessageManager
    {
        private OracleConnection _conn;

        public ForumMessageManager(OracleConnection conn) => _conn = conn;

        public bool Create(ForumMessage message, out string error)
        {
            var cmd = new OracleCommand("insert_message", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("pass_text", OracleDbType.Varchar2, 255).Value = message.Text;
            cmd.Parameters.Add("pass_topic_id", OracleDbType.Int32).Value = message.TopicId;
            cmd.Parameters.Add("pass_user_id", OracleDbType.Int32).Value = message.UserId;
            cmd.Parameters.Add("err", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            if(cmd.Parameters["result"].Value.ToString() == "1")
            {
                error = "";
                return true;
            }
            error = cmd.Parameters["err"].Value.ToString();
            return false;
        }
    }
}
