using Dapper;
using ISPLabs.Models;
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
    public class CategoryRepository : ICategoryRepository
    {
        private string connectionString;
        public CategoryRepository(string conn)
        {
            connectionString = conn;
        }

        public CategoryAPIModel Append(CategoryAPIModel category)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("INSERT INTO [dbo].[Category] (Name, Description, Partition_id) ");
                sql.Append("VALUES(@Name, @Description, @Partition_id);");
                var affectedRows = db.Execute(
                    sql.ToString(),
                    new { category.Name, category.Description, Partition_id = category.PartitionId}
                );
                if (affectedRows == 0)
                    return null;
                sql.Clear();
                sql.Append("SELECT * FROM [dbo].[Category] AS c ");
                sql.Append("WHERE c.Name = @Name AND c.Description = @Description;");
                return db.Query<CategoryAPIModel>(sql.ToString(), new { category.Name, category.Description }).FirstOrDefault();
            }
        }

        public HashSet<Category> GetAll()
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var categoryDictionary = new Dictionary<int, Category>();
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Category] AS c ");
                sql.Append("LEFT JOIN [dbo].[Topic] as t ON c.id = topic.Category_id;");
                return db.Query<Category, Topic, Category>(
                    sql.ToString(),
                    (category, topic) =>
                    {
                        Category categoryEntry;
                        if(!categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                        {
                            categoryEntry = category;
                            categoryEntry.Topics = new HashSet<Topic>();
                            categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                        }
                        categoryEntry.Topics.Add(topic);
                        return categoryEntry;
                    },
                    splitOn: "Id"
                ).ToHashSet();
            }
        }

        public HashSet<Category> GetAllWithoutChilds()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM [dbo].[Category];";
                return db.Query<Category>(sql).ToHashSet();
            }
        }

        public Category GetByIdWithoutChilds(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Category] AS c ");
                sql.Append("WHERE c.Id = @Id;");
                return db.Query<Category>(sql.ToString(), new { Id = id }).FirstOrDefault();
            }
        }

        public CategoryAPIModel GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM [dbo].[Category] AS c ");
                sql.Append("WHERE c.Id = @Id;");
                var category = db.Query<CategoryAPIModel>(sql.ToString(), new { Id = id }).FirstOrDefault();
                if (category == null)
                    return null;
                sql.Clear();
                var topicDictionary = new Dictionary<int, TopicAPIModel>();
                sql.Append("SELECT * FROM [dbo].[Topic] AS t ");
                sql.Append("LEFT JOIN [dbo].[ForumMessage] AS fm ON fm.Topic_id = t.Id ");
                sql.Append("WHERE t.Category_id = @Id;");
                category.Topics = db.Query<TopicAPIModel, MessageAPIModel, TopicAPIModel>(
                    sql.ToString(),
                    (topic, forumMessage) => {
                        TopicAPIModel topicEntry;
                        if(!topicDictionary.TryGetValue(topic.Id, out topicEntry))
                        {
                            topicEntry = topic;
                            topicEntry.MessagesCount = 0;
                            topicDictionary.Add(topicEntry.Id, topicEntry);
                        }
                        topicEntry.MessagesCount += 1;
                        return topicEntry;
                    },
                    new { Id  = id },
                    splitOn: "Id").ToHashSet();
                category.TopicsCount = category.Topics.Count;
                return category;
            }
        }

        public bool Remove(int id)
        {
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("DELETE FROM [dbo].[Category] ");
                sql.Append("WHERE Id = @Id;");
                return db.Execute(sql.ToString(), new { Id = id }) == 1;
            }
        }

        public bool Update(CategoryAPIModel category)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = new StringBuilder();
                sql.Append("UPDATE [dbo].[Category] ");
                sql.Append("SET Name = @Name, Description = @Description ");
                sql.Append("WHERE Id = @Id;");
                return db.Execute(sql.ToString(), category) == 1;
            }
        }
    }
}
