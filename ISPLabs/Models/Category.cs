using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class Category
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Partition Partition { get; set; }
        public virtual ISet<Topic> Topics { get; set; }
        public Category()
        {
            Topics = new HashSet<Topic>();
        }
    }
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
            References(x => x.Partition).Cascade.SaveUpdate();
            HasMany(x => x.Topics).Cascade.All().Inverse();
        }
    }
}
