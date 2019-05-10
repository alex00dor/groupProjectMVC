using System.ComponentModel.DataAnnotations;
using ChatProject.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatProject.Models.ViewModels
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Please enter a name. ")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please repeat a password")]
        [Compare("Password", ErrorMessage = "Different passwords.")]
        [Display(Name = "Repeat password")]
        public string Password2 { get; set; }
    }
}