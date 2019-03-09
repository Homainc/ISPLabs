using ISPLabs.Models;
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
                while (await reader.ReadAsync())
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

        public static Category Convert(DbDataReader reader)
        {
            var category = new Category();
            category.Id = Int32.Parse(reader["category_id"].ToString());
            category.Name = reader["category_name"].ToString();
            category.Description = reader["category_description"].ToString();
            category.PartitionId = Int32.Parse(reader["partition_id"].ToString());
            return category;
        }
    }
}
