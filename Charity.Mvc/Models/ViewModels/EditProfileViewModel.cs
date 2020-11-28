using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class EditProfileViewModel
    {
        private string _name;
        private string _surname;
        private string _phoneNumber;

        [Required]
        [Display(Prompt = "podaj imię")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        [Required]
        [Display(Prompt = "podaj nazwisko")]
        public string Surname { get => _surname; set => _surname = value?.Trim(); }

        public string Email { get; set; }

        [Display(Prompt = "podaj telefon", Name = "Telefon")]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value?.Trim(); }

        [Display(Prompt = "podaj obecne hasło")]
        public string CurrentPassword { get; set; }

        [Display(Prompt = "podaj nowe hasło")]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Prompt = "powtórz nowe hasło")]
        public string RepeatPassword { get; set; }
    }
}
