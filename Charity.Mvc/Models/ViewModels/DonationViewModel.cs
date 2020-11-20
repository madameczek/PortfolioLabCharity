using Charity.Mvc.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class DonationViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime PickUpDateOn { get; set; }
        public DateTime PickUpTimeOn { get; set; }
        public string PickUpComment { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<Institution>  Institutions { get; set; }
        public Institution SelectedInstitution { get; set; }
    }
}
