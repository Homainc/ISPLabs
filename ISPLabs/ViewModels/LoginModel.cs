using System.ComponentModel.DataAnnotations;

namespace ISPLabs.ViewModels
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "IncorrectEmail")]
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
