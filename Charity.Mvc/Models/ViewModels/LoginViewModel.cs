using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class LoginViewModel
    {
        private string _email;

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Nie wygląda to jak adres email")]
        public string Email { get => _email; set => _email = value?.Trim(); }

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
