using System.Collections.Generic;
using System.Linq;

namespace ISPLabs.Models.API
{
    public class CategoryAPIModel
    {
        public int Id { get; set; }
        public int PartitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ISet<TopicAPIModel> Topics { get; set; }
        public int TopicsCount { get; set; }
        public CategoryAPIModel() {}
        public CategoryAPIModel(Category cat, bool verbose = false)
        {
            Id = cat.Id;
            Name = cat.Name;
            PartitionId = cat.Partition.Id;
            TopicsCount = cat.Topics.Count();
            if (verbose)
                Topics = cat.Topics.Select(x => new TopicAPIModel(x, false)).ToHashSet();
            Description = cat.Description;
        }
    }
}
