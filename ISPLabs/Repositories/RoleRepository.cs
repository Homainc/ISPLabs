using Dapper;
using ISPLabs.Models;
using ISPLabs.Models.API;
using ISPLabs.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ISPLabs.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private string connectionString;
        public RoleRepository(string conn) => connectionString = conn;
        public HashSet<RoleAPIModel> GetAll()
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Role];");
                return db.Query<RoleAPIModel>(sql.ToString()).ToHashSet();
            }
        }

        public Role GetByName(string name)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Role] AS r ");
                sql.Append("WHERE r.Name = @Name");
                return db.Query<Role>(sql.ToString(), new { Name = name }).FirstOrDefault();
            }
        }
    }
}
