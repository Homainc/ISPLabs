using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class RoleManager
    {
        private OracleConnection _conn;

        public RoleManager(OracleConnection conn) => _conn = conn;

        public async Task<Role> GetByNameAsync(string name)
        {
            var cmd = new OracleCommand("get_role_by_name", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("pass_name", OracleDbType.Varchar2, 255).Value = name;
            cmd.Parameters.Add("role_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("role_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            var role = RoleManager.Convert(cmd.Parameters);
            return role;
        }

        public static Role Convert(OracleParameterCollection item)
        {
            var role = new Role();
            role.Id = Int32.Parse(item["role_id"].Value.ToString());
            role.Name = item["role_name"].Value.ToString();
            return role;
        }

        public static Role Convert(DbDataReader reader)
        {
            var role = new Role();
            role.Id = Int32.Parse(reader["role_id"].ToString());
            role.Name = reader["role_name"].ToString();
            return role;
        }
    }
}
