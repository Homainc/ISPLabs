using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "LoginRequired")]
        public string Login { get; set; }

        [EmailAddress(ErrorMessage = "IncorrectEmail")]
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "IncorrectConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
