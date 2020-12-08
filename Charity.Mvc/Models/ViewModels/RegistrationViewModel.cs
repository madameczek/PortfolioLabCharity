using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class RegistrationViewModel
    {
        private string _name;
        private string _surname;
        private string _email;
        private string _phoneNumber;

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "podaj imię")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "podaj nazwisko")]
        public string Surname { get => _surname; set => _surname = value?.Trim(); }

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nie wygląda to jak adres email")]
        public string Email { get => _email; set => _email = value?.Trim(); }

        [Display(Prompt = "podaj telefon", Name = "Telefon")]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value?.Trim(); }

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Display(Prompt = "podaj hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste.")]
        [Compare("Password")]
        [Display(Prompt = "powtórz hasło")]
        public string RepeatPassword { get; set; }
    }
}
