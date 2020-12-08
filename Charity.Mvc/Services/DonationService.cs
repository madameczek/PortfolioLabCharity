using Charity.Mvc.Contexts;
using Charity.Mvc.Models.DbModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Charity.Mvc.Services
{
    public class DonationService : IDonationService
    {
#region Ctor & DI
        private readonly CharityDbContext _context;
        private readonly ILogger _logger;
        public DonationService(ILoggerFactory loggerFactory, CharityDbContext context)
        {
            _logger = loggerFactory.CreateLogger("Donation Service");
            _context = context;
        }
#endregion

        public List<InstitutionModel> GetInstitutionList(int skip = 0, int take = 4)
        {
            try
            {
                return take != 0 ? _context.Institutions.Skip(skip).Take(take).ToList() : _context.Institutions.ToList();
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
                return _context.Institutions.FirstOrDefault(i => i.Id == id);
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

        public List<DonationModel> GetDonations(string userId)
        {
            try
            {
                var donations = _context.Donations
                    .Where(d => d.User.Id == userId)
                    .OrderByDescending(d => d.PickUpOn)
                    .Include(d => d.CategoryDonation)
                        .ThenInclude(c => c.Category)
                    .Include(d => d.Institution);
                return donations.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching donation list for {User}", userId);
                throw;
            }
        }

        public Task CreateDonationAsync(DonationModel donation, int institutionId, List<int> categoryIds)
        {
            try
            {
                // Create donation entity
                donation.Institution = GetInstitution(institutionId);
                _context.Donations.Add(donation);
                var result = _context.SaveChanges();
                // Create entities for joining table between donations and categories
                var categoryDonations = new List<CategoryDonationModel>();
                categoryIds.ForEach(c => categoryDonations.Add(new CategoryDonationModel() { CategoryId = c, DonationId = donation.Id }));
                _context.AddRange(categoryDonations);
                result += _context.SaveChanges();

                if (result > 1) _logger.LogInformation("Donation created in the database.");

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
