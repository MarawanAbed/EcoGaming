using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests
{
    public class ResetPasswordActionRequest
    {
        [MinLength(6, ErrorMessage = "Min len 6")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Min len 6")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }

        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
