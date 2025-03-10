using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests
{
    public class RegisterActionRequest
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name Required")]
        [MaxLength(50, ErrorMessage = "Min Len 50")]
        public string UserName { get; set; }

        [MinLength(6, ErrorMessage = "Min len 6")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Min len 6")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role Required")]
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; } = new(); // List to hold roles dynamically


    }
}
