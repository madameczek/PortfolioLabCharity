using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class RegistrationViewModel
    {
        private string _name;
        private string _surname;
        private string _email;

        [Required]
        [Display(Prompt = "podaj imię")]
        public string Name { get => _name; set => _name = value.Trim(); }

        [Required]
        [Display(Prompt = "podaj nazwisko")]
        public string Surname { get => _surname; set => _surname = value.Trim(); }

        [Required]
        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nie wygląda to jak adres email")]
        public string Email { get => _email; set => _email = value.Trim(); }

        [Required]
        [Display(Prompt = "podaj hasło")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Prompt = "powtórz hasło")]
        public string RepeatPassword { get; set; }
    }
}
