using Charity.Mvc.Models.DbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class OrganisationsBagsViewModel
    {
        public List<InstitutionModel> Institutions { get; set; }
        public int BagCount { get; set; }
        public int InstitutionCount { get; set; }
    }
}
