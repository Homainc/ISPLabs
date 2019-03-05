using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Partition Partition { get; set; }
        public ISet<Topic> Topics { get; set; }
        public Category()
        {
            Topics = new HashSet<Topic>();
        }
    }
}
