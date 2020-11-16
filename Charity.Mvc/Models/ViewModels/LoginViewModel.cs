using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Prompt = "Enter your email")]
        public string Email { get; set; }
        [Required]
        [Display(Prompt = "Enter password")]
        public string Password { get; set; }
    }
}
