using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class Topic
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsClosed { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Category Category { get; set; }
        public virtual ISet<ForumMessage> Messages { get; set; }
        public virtual User User { get; set; }
        public Topic()
        {
            Messages = new HashSet<ForumMessage>();
        }
    }
    public class TopicMap : ClassMap<Topic>
    {
        public TopicMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Date);
            Map(x => x.IsClosed);
            References(x => x.Category).Cascade.SaveUpdate();
            HasMany(x => x.Messages).Cascade.All().Inverse();
            References(x => x.User).Cascade.SaveUpdate();
        }
    }
}