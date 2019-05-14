using ISPLabs.Models;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class CategoryManager
    {
        private OracleConnection _conn;
        private string _lastError;

        public string LastError { get { return _lastError; } }

        public CategoryManager(OracleConnection conn) => _conn = conn;

        public async Task<Category> GetByIdAsync(int id)
        {
            OracleCommand cmd = new OracleCommand("get_category", _conn);
            cmd.CommandType =  CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("result_topics", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("category_name", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            Category category = new Category();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.HasRows && await reader.ReadAsync())
                {
                    var topic = TopicManager.Convert(reader);
                    topic.LastActivity = reader["last_message_date"] as DateTime? ?? topic.Date;
                    topic.MessagesCount = Int32.Parse(reader["messages_count"].ToString());
                    category.Topics.Add(topic);
                }
                category.Name = cmd.Parameters["category_name"].Value.ToString();
            }
            return category;
        }

        public async Task<bool> CreateAsync(Category category)
        {
            var cmd = OracleHelper.SetupProcCmd("insert_category", _conn);
            CategoryManager.AppendInPars(cmd, category, false);
            cmd.Parameters.Add("category_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            if (OracleHelper.BoolResultWithError(cmd, out _lastError))
            {
                category.Id = Int32.Parse(cmd.Parameters["category_id"].Value.ToString());
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var cmd = OracleHelper.SetupProcCmd("update_category", _conn);
            CategoryManager.AppendInPars(cmd, category, true);
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cmd = OracleHelper.SetupProcCmd("delete_category", _conn);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }

        public static Category Convert(DbDataReader reader)
        {
            var category = new Category();
            category.Id = Int32.Parse(reader["category_id"].ToString());
            category.Name = reader["category_name"].ToString();
            category.Description = reader["category_description"].ToString();
            category.PartitionId = Int32.Parse(reader["partition_id"].ToString());
            return category;
        }

        public static void AppendInPars(OracleCommand cmd, Category category, bool id)
        {
            if (id)
                cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = category.Id;
            cmd.Parameters.Add("p_partition_id", OracleDbType.Int32).Value = category.PartitionId;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2, 255).Value = category.Name;
            cmd.Parameters.Add("p_description", OracleDbType.Varchar2, 255).Value = category.Description;
        }
    }
}
