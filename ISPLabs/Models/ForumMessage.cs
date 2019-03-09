using System;

namespace ISPLabs.Models
{
    public class ForumMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Topic Topic { get; set; }
        public int TopicId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public ForumMessage()
        {
            Topic = new Topic();
            User = new User();
        }

        public ForumMessage(string text, int topicId, int userId)
        {
            Text = text;
            TopicId = topicId;
            UserId = userId;
            User = new User();
            Topic = new Topic();
        }
    }
}
