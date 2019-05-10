using System.ComponentModel.DataAnnotations;

namespace ChatProject.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter a email")]
        [EmailAddress]
        [UIHint("email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}