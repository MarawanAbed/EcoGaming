using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests
{
    public class ForgetPasswordActionRequest
    {

        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }
    }
}
