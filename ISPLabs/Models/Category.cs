using System.Collections.Generic;

namespace ISPLabs.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PartitionId { get; set; }
        public Partition Partition { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public int TopicCount { get; set; }

        public Category()
        {
            Topics = new List<Topic>();
        }

        public Category(int id, string name, string description, int topicCount,Partition partition)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Partition = new Partition();
            this.PartitionId = partition.Id;
            this.TopicCount = topicCount;
            this.Topics = new List<Topic>();
        }
    }
}
