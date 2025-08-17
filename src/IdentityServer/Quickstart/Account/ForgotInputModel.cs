using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Quickstart.Account
{
    public class ForgotInputModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}