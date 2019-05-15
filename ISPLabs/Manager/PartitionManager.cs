using ISPLabs.Models;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Manager
{
    public class PartitionManager
    {
        private OracleConnection _conn;
        private string _lastError;

        public string LastError { get { return _lastError; } }

        public PartitionManager(OracleConnection conn) => _conn = conn;

        public async Task<ICollection<Partition>> GetAllAsync()
        {
            OracleCommand cmd = new OracleCommand("get_partition_eager", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("resultItems", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            Dictionary<int, Partition> dict = new Dictionary<int, Partition>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var pid = Int32.Parse(reader["partition_id"].ToString());
                    Partition partition;
                    if (!dict.TryGetValue(pid, out partition))
                    {
                        partition = new Partition(pid, reader["partition_name"].ToString());
                        dict.Add(pid, partition);
                    }
                    if (reader["category_id"] != DBNull.Value)
                    {
                        var category = CategoryManager.Convert(reader);
                        category.TopicCount = Int32.Parse(reader["topic_count"].ToString());
                        partition.Categories.Add(category);
                    }
                }
            }
            return dict.Values.ToList();
        }

        public async Task<bool> CreateAsync(Partition partition, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = OracleHelper.SetupProcCmd("insert_partition", _conn);
            cmd.Parameters.Add("partition_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2, 255).Value = partition.Name;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            partition.Id = Int32.Parse(cmd.Parameters["partition_id"].Value.ToString());
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }

        public async Task<bool> UpdateAsync(Partition partition, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = OracleHelper.SetupProcCmd("update_partition", _conn);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = partition.Id;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2, 255).Value = partition.Name;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        
        }

        public async Task<bool> DeleteAsync(int id, string username)
        {
            OracleHelper.AddLoginContext(username, _conn);
            var cmd = OracleHelper.SetupProcCmd("delete_partition", _conn);
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("er", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;
            await cmd.ExecuteNonQueryAsync();
            return OracleHelper.BoolResultWithError(cmd, out _lastError);
        }
    }
}
