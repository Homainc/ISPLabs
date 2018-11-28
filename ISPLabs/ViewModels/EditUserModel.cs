using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.ViewModels
{
    public class EditUserModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "LoginRequired")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "IncorrectEmail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
