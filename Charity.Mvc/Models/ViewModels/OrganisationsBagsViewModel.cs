using Charity.Mvc.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    // Used in Slide1 on Home/Index
    public class OrganisationsBagsViewModel
    {
        public List<Institution> Institutions { get; set; }
        public int BagCount { get; set; }
        public int InstitutionCount { get; set; }
    }
}
