using Dapper;
using ISPLabs.Models.API;
using ISPLabs.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPLabs.Repositories
{
    public class PartitionRepository : IPartitionRepository
    {
        private string connectionString;
        public PartitionRepository(string conn) => connectionString = conn;

        public PartitionAPIModel Append(PartitionAPIModel partition)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("INSERT INTO [dbo].[Partition] ");
                sql.Append("(Name) ");
                sql.Append("VALUES(@Name);");
                var affectedRows = db.Execute(sql.ToString(), partition);
                if (affectedRows == 0)
                    return null;
                sql.Clear();
                sql.Append("SELECT * FROM [dbo].[Partition] ");
                sql.Append("WHERE Name = @Name;");
                return db.Query<PartitionAPIModel>(sql.ToString(), partition).FirstOrDefault();
            }
        }

        public HashSet<PartitionAPIModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var partitionDictionary = new Dictionary<int, Dictionary<int, CategoryAPIModel>>();
                var partitions = new Dictionary<int, PartitionAPIModel>();
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Partition] AS p ");
                sql.Append("LEFT JOIN [dbo].[Category] AS c ON p.Id = c.Partition_id ");
                sql.Append("LEFT JOIN [dbo].[Topic] AS t ON c.Id = t.Category_id;");
                db.Query<PartitionAPIModel, CategoryAPIModel, TopicAPIModel, PartitionAPIModel>(
                    sql.ToString(),
                    (partition, category, topic) =>
                    {
                        Dictionary<int, CategoryAPIModel> categoryDictionary;
                        if (!partitionDictionary.TryGetValue(partition.Id, out categoryDictionary))
                        {
                            categoryDictionary = new Dictionary<int, CategoryAPIModel>();
                            partitionDictionary.Add(partition.Id, categoryDictionary);
                            partitions.Add(partition.Id, partition);
                            partitions[partition.Id].CategoriesCount = 0;
                            partitions[partition.Id].Categories = new HashSet<CategoryAPIModel>();
                        }
                        CategoryAPIModel categoryEntry = null;
                        if (category != null && !categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                        {
                            categoryEntry = category;
                            partitions[partition.Id].Categories.Add(categoryEntry);
                            partitions[partition.Id].CategoriesCount += 1;
                            categoryEntry.TopicsCount = 0;
                            categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                        }
                        if (categoryEntry != null && topic != null)
                            categoryEntry.TopicsCount += 1;
                        return partition;
                    },
                    splitOn: "Id").ToHashSet();
                var result = new HashSet<PartitionAPIModel>();
                foreach(var p in partitions.Values)
                    result.Add(p);
                return result;
            }

        }

        public PartitionAPIModel GetByIdWithoutChilds(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Partition] ");
                sql.Append("WHERE Id = @Id;");
                return db.Query<PartitionAPIModel>(sql.ToString(), new { Id = id }).FirstOrDefault();
            }
        }

        public bool Update(PartitionAPIModel partition)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("UPDATE [dbo].[Partition] ");
                sql.Append("SET Name = @Name ");
                sql.Append("WHERE Id = @Id;");
                return db.Execute(sql.ToString(), partition) == 1;
            }
        }
    }
}
