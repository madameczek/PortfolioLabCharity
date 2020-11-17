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

        public List<Institution> GetInstitutionList(int skip = 0, int take = 4)
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

        public List<Category> GetCategoryList()
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
    }
}
