using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Models.API
{
    public class RoleAPIModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleAPIModel() { }
        public RoleAPIModel(Role r)
        {
            Id = r.Id;
            Name = r.Name;
        }
    }
}
