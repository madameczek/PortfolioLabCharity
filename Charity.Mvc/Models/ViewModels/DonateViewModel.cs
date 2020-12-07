using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class DonateViewModel
    {
        [Range(1, 1000, ErrorMessage ="Odbieramy od jednego worka. Zaznacz przynajmniej 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Podaj ulicę")]
        [StringLength(150, ErrorMessage = "Tekst jest zbyt długi")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Podaj miasto")]
        [StringLength(50, ErrorMessage = "Tekst jest zbyt długi")]
        public string City { get; set; }

        [Required(ErrorMessage = "Podaj kod pocztowy")]
        [StringLength(10, ErrorMessage = "Tekst jest zbyt długi")]
        public string ZipCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50, ErrorMessage = "Tekst jest zbyt długi")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Wprowadź datę odbioru paczki")]
        public DateTime PickUpDateOn { get; set; }

        [DataType(DataType.Time, ErrorMessage = "Prowadź godzinę")]
        public DateTime PickUpTimeOn { get; set; }

        public string PickUpComment { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Musisz wybrać jedną instytucję")]
        public int InstitutionId { get; set; }

        public List<InstitutionViewModel> Institutions { get; set; }

        public string Command { get; set; }
    }
}
