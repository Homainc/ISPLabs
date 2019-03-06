using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class UserManager
    {
        private OracleConnection _conn;

        public UserManager(OracleConnection conn) => _conn = conn;

        public async Task<ICollection<User>> GetAllAsync()
        {
            OracleCommand cmd = new OracleCommand("get_users", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result_users", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            List<User> list = new List<User>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var user = UserManager.Convert(reader);
                    user.Role = RoleManager.Convert(reader);
                    list.Add(user);
                }
            }
            return list;
        }

        public async Task<User> GetByEmailAsync(string email) {
            var cmd = new OracleCommand("get_user_by_email", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("u_email", OracleDbType.Varchar2, 255).Value = email;
            cmd.Parameters.Add("user_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_login", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_email", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_password", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_reg_date", OracleDbType.Date).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("role_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("role_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            var user = UserManager.Convert(cmd.Parameters);
            user.Role = RoleManager.Convert(cmd.Parameters);
            return user;
        }

        public bool Login(string email, string password, out string error)
        {
            OracleCommand cmd = new OracleCommand("login", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("pass_email", OracleDbType.Varchar2, 255).Value = email;
            cmd.Parameters.Add("pass_password", OracleDbType.Varchar2, 255).Value = password;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            error = cmd.Parameters["er"].Value.ToString();
            return cmd.Parameters["result"].Value.ToString() == "1";
        }

        public static User Convert(OracleParameterCollection item, bool isSecure = false)
        {
            var user = new User();
            user.Id = Int32.Parse(item["user_id"].Value.ToString());
            user.Login = item["user_login"].Value.ToString();
            user.Email = item["user_email"].Value.ToString();
            user.Password = isSecure? item["user_password"].Value.ToString():null;
            user.RegistrationDate = ((OracleDate)item["user_reg_date"].Value).Value;
            user.RoleId = Int32.Parse(item["role_id"].Value.ToString());
            return user;
        }

        public static User Convert(DbDataReader reader, bool isSecure = false)
        {
            var user = new User();
            user.Id = Int32.Parse(reader["user_id"].ToString());
            user.Email = reader["user_email"].ToString();
            user.Login = reader["user_login"].ToString();
            user.Password = isSecure? reader["user_password"].ToString():null;
            user.RegistrationDate = (DateTime)reader["user_reg_date"];
            user.RoleId = Int32.Parse(reader["role_id"].ToString());
            return user;
        }
    }
}
