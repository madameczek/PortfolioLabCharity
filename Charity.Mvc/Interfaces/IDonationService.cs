using Charity.Mvc.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        List<InstitutionModel> GetInstitutionList(int skip = 0, int take = 4);
        public InstitutionModel GetInstitution(int id);
        int GetTotalNumberOfBags();
        int GetInstitutionCount();
        List<CategoryModel> GetCategoryList();
        public Task CreateDonationAsync(DonationModel donation, int institutionId, List<int> categoryIds);
    }
}