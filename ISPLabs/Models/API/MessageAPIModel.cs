using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Models.API
{
    public class MessageAPIModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ForumUserAPIModel User { get; set; }
        public int TopicId { get; set; } 
        public string Date { get; set; }
        public MessageAPIModel(){}
        public MessageAPIModel(ForumMessage msg)
        {
            Id = msg.Id;
            Text = msg.Text;
            User = new ForumUserAPIModel(msg.User);
            TopicId = msg.Topic.Id;
            Date = $"{msg.Date.ToShortDateString()} {msg.Date.ToShortTimeString()}";
        }
    }
}
