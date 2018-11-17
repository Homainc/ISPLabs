using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISPLabs.Models;

namespace ISPLabs.Models.API
{
    public class PartitionAPIModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ISet<CategoryAPIModel> Categories { get; set; }
        public int CategoriesCount { get; set; }
        public PartitionAPIModel() { }
        public PartitionAPIModel(Partition p)
        {
            Id = p.Id;
            Name = p.Name;
            CategoriesCount = p.Categories.Count();
            Categories = p.Categories.Select(x => new CategoryAPIModel(x)).ToHashSet();
        }
    }
}
