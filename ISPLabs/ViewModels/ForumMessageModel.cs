using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.ViewModels
{
    public class ForumMessageModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } 
        public string Text { get; set; }
        public string UserLogin { get; set; }
        public bool IsTopicOwner { get; set; }
        public int UserMessages { get; set; }
        public string UserRole { get; set; }
    }
}
