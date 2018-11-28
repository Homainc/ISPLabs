using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Models.API
{
    public class TopicAPIModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ISet<MessageAPIModel> Messages { get; set; }
        public int MessagesCount { get; set; }
        public bool IsClosed { get; set; }
        public string LastActivity { get; set; }
        public TopicAPIModel(){}
        public TopicAPIModel(Topic topic, bool verbose = false)
        {
            Id = topic.Id;
            Name = topic.Name;
            if(verbose)
                Messages = topic.Messages.Select(x => new MessageAPIModel(x)).ToHashSet();
            MessagesCount = topic.Messages.Count();
            IsClosed = topic.IsClosed;
            var lastmsg = topic.Messages.OrderBy(x => x.Date).LastOrDefault();
            LastActivity = lastmsg == null ? "null" : $"{lastmsg.Date.ToShortDateString()} {lastmsg.Date.ToShortTimeString()}"; 

        }
    }
}
