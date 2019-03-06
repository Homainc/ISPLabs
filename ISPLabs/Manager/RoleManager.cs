using ISPLabs.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.Common;

namespace ISPLabs.Manager
{
    public class RoleManager
    {
        private OracleConnection _conn;

        public RoleManager(OracleConnection conn) => _conn = conn;

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
