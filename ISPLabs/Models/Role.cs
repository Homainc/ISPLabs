using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ISPLabs.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }

        public Role(int id, string name) : this()
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
