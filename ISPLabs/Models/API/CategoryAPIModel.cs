using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Models.API
{
    public class CategoryAPIModel
    {
        public int Id { get; set; }
        public int PartitionId { get; set; }
        public string Name { get; set; }
        public int Topics { get; set; }
        public CategoryAPIModel() { }
        public CategoryAPIModel(Category cat)
        {
            Id = cat.Id;
            Name = cat.Name;
            PartitionId = cat.Partition.Id;
            Topics = cat.Topics.Count();
        }
    }
}
