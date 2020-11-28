using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class EditProfileViewModel
    {
        private string _name;
        private string _surname;
        private string _email;
        private string _phoneNumber;

        public string Id { get; set; }

        [Required]
        [Display(Prompt = "podaj imię")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        [Required]
        [Display(Prompt = "podaj nazwisko")]
        public string Surname { get => _surname; set => _surname = value?.Trim(); }

        public string Email { get => _email; set => _email = value?.Trim(); }

        [Display(Prompt = "podaj telefon", Name = "Telefon")]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value?.Trim(); }

        [Display(Prompt = "podaj hasło")]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Prompt = "powtórz hasło")]
        public string RepeatPassword { get; set; }
    }
}
