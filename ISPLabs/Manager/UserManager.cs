using ISPLabs.Models;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class UserManager
    {
        private OracleConnection _conn;
        private string _lastError;

        public string LastError { get { return _lastError; } }

        public UserManager(OracleConnection conn) => _conn = conn;

        public async Task<ICollection<User>> GetAllAsync()
        {
            OracleCommand cmd = OracleHelper.SetupProcCmd("get_users", _conn, false);
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
            var cmd = OracleHelper.SetupProcCmd("get_user_by_email", _conn, false);
            cmd.Parameters.Add("u_email", OracleDbType.Varchar2, 255).Value = email;
            UserManager.AppendOutPars(cmd);
            cmd.Parameters.Add("user_password", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            RoleManager.AppendOutPars(cmd);
            await cmd.ExecuteNonQueryAsync();
            var user = UserManager.Convert(cmd.Parameters);
            user.Role = RoleManager.Convert(cmd.Parameters);
            return user;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            OracleCommand cmd = OracleHelper.SetupProcCmd("login", _conn);
            cmd.Parameters.Add("pass_email", OracleDbType.Varchar2, 255).Value = email;
            cmd.Parameters.Add("pass_password", OracleDbType.Varchar2, 255).Value = password;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }

        public async Task<bool> RegistrationAsync(User user)
        {
            OracleCommand cmd = OracleHelper.SetupProcCmd("registration", _conn);
            cmd.Parameters.Add("pass_email", OracleDbType.Varchar2, 255).Value = user.Email;
            cmd.Parameters.Add("pass_password", OracleDbType.Varchar2, 255).Value = user.Password;
            cmd.Parameters.Add("pass_login", OracleDbType.Varchar2, 255).Value = user.Login;
            cmd.Parameters.Add("pass_role_id", OracleDbType.Int32).Value = user.Role.Id;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }

        public static User Convert(OracleParameterCollection item, bool isSecure = false)
        {
            var user = new User();
            user.Id = item["user_id"] == null ? 0 : Int32.Parse(item["user_id"].Value.ToString());
            user.Login = item["user_login"] == null ? null : item["user_login"].Value.ToString();
            user.Email = item["user_email"] == null ? null : item["user_email"].Value.ToString();
            user.Password = isSecure ? item["user_password"].Value.ToString() : null;
            user.RegistrationDate = item["user_reg_date"] == null ? default(DateTime) : ((OracleDate)item["user_reg_date"].Value).Value;
            user.RoleId = item["role_id"] == null ? 0 : Int32.Parse(item["role_id"].Value.ToString());
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

        public static void AppendOutPars(OracleCommand cmd)
        {
            cmd.Parameters.Add("user_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_login", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_email", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("user_reg_date", OracleDbType.Date).Direction = ParameterDirection.Output;
        }
    }
}
