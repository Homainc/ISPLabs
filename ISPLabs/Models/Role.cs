using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ISPLabs.Models
{
    [DataContract]
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ISet<User> Users { get; set; }
        public Role()
        {
            Users = new HashSet<User>();
        }
    }
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            HasMany(x => x.Users).Inverse();
        }
    }
}
