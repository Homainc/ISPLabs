using System;
using System.Collections.Generic;

namespace ISPLabs.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; } 
        public ICollection<Topic> Topics { get; set; }
        public ICollection<ForumMessage> Messages { get; set; }

        public User()
        {
            Topics = new List<Topic>();
            Messages = new List<ForumMessage>();
            Role = new Role();
        }

        public User(int id, string login, string email, DateTime regDate, Role role)
        {
            this.Id = id;
            this.Login = login;
            this.Email = email;
            this.RegistrationDate = regDate;
            this.Role = role;
            this.RoleId = role.Id;
            this.Topics = new List<Topic>();
            this.Messages = new List<ForumMessage>();
        }
    }
}
