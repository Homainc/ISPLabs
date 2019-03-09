using ISPLabs.Models;
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
                    var category = CategoryManager.Convert(reader);
                    category.TopicCount = Int32.Parse(reader["topic_count"].ToString());
                    partition.Categories.Add(category);
                }
            }
            return dict.Values.ToList();
        }
    }
}
