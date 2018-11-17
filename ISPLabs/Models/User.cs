using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime RegistrationDate { get; set; }
        public virtual Role Role { get; set; }
        public virtual ISet<Topic> Topics { get; set; }
        public virtual ISet<ForumMessage> Messages { get; set; }
        public User()
        {
            Topics = new HashSet<Topic>();
            Messages = new HashSet<ForumMessage>();
        }
    }
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Login).Unique();
            Map(x => x.Email).Unique();
            Map(x => x.Password);
            Map(x => x.RegistrationDate);
            References(x => x.Role).Cascade.SaveUpdate();
            HasMany(x => x.Topics).Cascade.All().Inverse();
            HasMany(x => x.Messages).Cascade.All().Inverse();
        }
    }
}
