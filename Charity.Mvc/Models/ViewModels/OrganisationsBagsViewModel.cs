using Charity.Mvc.Models.DbModels;
using System.Collections.Generic;

namespace Charity.Mvc.Models.ViewModels
{
    // Used in Slide1 on Home/Index
    public class OrganisationsBagsViewModel
    {
        public List<InstitutionModel> Institutions { get; set; }
        public int BagCount { get; set; }
        public int InstitutionCount { get; set; }
    }
}
