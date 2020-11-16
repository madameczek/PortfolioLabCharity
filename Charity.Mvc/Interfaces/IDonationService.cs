using Charity.Mvc.Models.DbModels;
using System.Collections.Generic;

namespace Charity.Mvc.Services
{
    public interface IDonationService
    {
        List<Institution> GetInstitutionList(int skip = 0, int take = 4);
        int GetTotalNumberOfBags();
        int GetInstitutionCount();
    }
}