using System;
using System.Collections.Generic;

namespace ISPLabs.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public DateTime Date { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ForumMessage> Messages { get; set; }
        public User User { get; set; }

        public Topic()
        {
            Messages = new List<ForumMessage>();
            Category = new Category();
        }
    }
}