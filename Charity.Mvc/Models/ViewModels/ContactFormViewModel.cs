using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Pole nie może być puste.")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Pole nie może być puste.")]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Pole nie może być puste.")]
        [MaxLength(300)]
        public string Message { get; set; }
    }
}
