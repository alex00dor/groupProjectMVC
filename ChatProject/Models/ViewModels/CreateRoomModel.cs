using System.ComponentModel.DataAnnotations;

namespace ChatProject.Models.ViewModels
{
    public class CreateRoomModel
    {    
        [Required(ErrorMessage = "Enter a name of room")]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Describe your room")]
        [StringLength(600, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Description { get; set; }
    }
}