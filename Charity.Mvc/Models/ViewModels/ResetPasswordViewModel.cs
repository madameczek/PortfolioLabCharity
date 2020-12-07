using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Required]
        [Display(Prompt = "podaj hasło")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Prompt = "powtórz hasło")]
        public string RepeatPassword { get; set; }
    }
}
