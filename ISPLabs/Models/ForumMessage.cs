using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ISPLabs.Models
{
    public class ForumMessage
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
    }
    public class ForumMessageMap : ClassMap<ForumMessage>
    {
        public ForumMessageMap()
        {
            Id(x => x.Id);
            Map(x => x.Text);
            Map(x => x.Date);
            References(x => x.Topic).Cascade.SaveUpdate();
            References(x => x.User).Cascade.SaveUpdate();
        }
    }
}
