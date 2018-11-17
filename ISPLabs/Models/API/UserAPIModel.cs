using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Models.API
{
    public class UserAPIModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserAPIModel() { }
        public UserAPIModel(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Email = user.Email;
            Role = user.Role.Name;
            Password = user.Password;
            RegistrationDate = user.RegistrationDate;
        }
    }
}
