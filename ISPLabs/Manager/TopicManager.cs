using ISPLabs.Models;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data.Common;

namespace ISPLabs.Manager
{
    public class TopicManager
    {
        public static Topic Convert(DbDataReader reader)
        {
            var topic = new Topic();
            topic.Id = Int32.Parse(reader["topic_id"].ToString());
            topic.Name = reader["topic_name"].ToString();
            topic.Date = (DateTime)reader["topic_date"];
            topic.CategoryId = Int32.Parse(reader["category_id"].ToString());
            topic.IsClosed = reader["topic_is_closed"].ToString() == "1";
            return topic;
        }
    }
}
