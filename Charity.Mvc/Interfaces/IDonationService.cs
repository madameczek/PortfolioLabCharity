using Charity.Mvc.Models.DbModels;
using System.Collections.Generic;

namespace Charity.Mvc.Services
{
    public interface IDonationService
    {
        /// <summary>
        /// Reads list of institutions from a database. If parameter = 0, returns all institutions.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>Returns list of institutions.</returns>
        List<Institution> GetInstitutionList(int skip = 0, int take = 4);
        int GetTotalNumberOfBags();
        int GetInstitutionCount();
    }
}