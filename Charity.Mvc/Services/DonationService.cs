using Charity.Mvc.Contexts;
using Charity.Mvc.Models.DbModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class DonationService : IDonationService
    {
        private readonly CharityDbContext _context;
        private readonly ILogger _logger;
        public DonationService(ILoggerFactory loggerFactory, CharityDbContext context)
        {
            _logger = loggerFactory.CreateLogger("Donation Service");
            _context = context;
        }

        public List<InstitutionModel> GetInstitutionList(int skip = 0, int take = 4)
        {
            try
            {
                if (take != 0)
                {
                    return _context.Institutions.Skip(skip).Take(take).ToList();
                }
                else
                {
                    return _context.Institutions.ToList();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching institution list");
                throw;
            }
        }

        public InstitutionModel GetInstitution(int id)
        {
            try
            {
                return _context.Institutions.Where(i => i.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching institution {id}", id);
                throw;
            }
        }

        public int GetInstitutionCount()
        {
            try
            {
                return _context.Institutions.Count();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching institution count");
                throw;
            }
        }

        public int GetTotalNumberOfBags()
        {
            try
            {
                return _context.Donations.Sum(q => q.Quantity);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error fetching institution count");
                throw;
            }
        }

        public List<CategoryModel> GetCategoryList()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching category list");
                throw;
            }
        }

        public Task CreateDonationAsync(DonationModel donation, int institutionId, List<int> categoryIds)
        {
            try
            {
                donation.Institution = GetInstitution(institutionId);
                _context.Donations.Add(donation);
                var result = _context.SaveChanges();
                
                var categoryDonations = new List<CategoryDonationModel>();
                categoryIds.ForEach(c => categoryDonations.Add(new CategoryDonationModel() { CategoryId = c, DonationId = donation.Id }));
                _context.AddRange(categoryDonations);
                _context.SaveChanges();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating a donation.");
                return Task.FromException(e);
            }

        }
   
    }
}
