using System.Linq;

namespace ISPLabs.Models.API
{
    public class ForumUserAPIModel
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int MessagesCount { get; set; }
        public ForumUserAPIModel(){}
        public ForumUserAPIModel(User user)
        {
            Login = user.Login;
            Role = user.Role.Name;
            MessagesCount = user.Messages.Count();
            Email = user.Email;
        }
    }
}
