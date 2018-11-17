using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login not specified")]
        public string Login { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password not specified")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password is incorrect")]
        public string ConfirmPassword { get; set; }
    }
}
