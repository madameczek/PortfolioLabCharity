using Charity.Mvc.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class IndexOrganisationsBagsViewModel
    {
        public List<Institution> Institutions { get; set; }
        public int BagCount { get; set; }
        public int InstitutionCount { get; set; }
    }
}
