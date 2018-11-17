using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class Partition
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ISet<Category> Categories { get; set; }
        public Partition()
        {
            Categories = new HashSet<Category>();
        }
    }
    public class PartitionMap : ClassMap<Partition>
    {
        public PartitionMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Unique();
            HasMany(x => x.Categories).Cascade.All().Inverse();
        }
    }
}
