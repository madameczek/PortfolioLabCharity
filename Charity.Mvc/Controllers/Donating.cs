using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Charity.Mvc.Controllers
{
    [RequireHttps]
    public class Donating : Controller
    {
        private List<string> errors;

        [HttpGet]
        public IActionResult Donate()
        {
            try
            {
                // Prepare ViewModel with categories and institutions to be used when user makes a selection
                var categories = JsonConvert.SerializeObject(
                    _donationService.GetCategoryList(), 
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var institutions = JsonConvert.SerializeObject(
                    _donationService.GetInstitutionList(take: 0),
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var model = new DonationViewModel()
                {
                    Institutions = JsonConvert.DeserializeObject<List<InstitutionViewModel>>(institutions),
                    Categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(categories),
                };
                if(model.PickUpDateOn ==  DateTime.MinValue) { model.PickUpDateOn = DateTime.Now.AddDays(3); }
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {Action}", nameof(Donate));
                throw;
            }
        }

        [HttpPost]
        public IActionResult Donate(DonationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(!IsModelValid(model, out errors))
            {
                errors.ForEach(e => ModelState.AddModelError("", e));
                return View(model);
            }

            return View("Summary", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation(DonationViewModel model)
        {
            if (model.Command == "Edit")
            {
                return View("Donate", model);
            }

            // Prepare Donation object for save to database
            var donationJson = JsonConvert.SerializeObject(model);
            var donation = JsonConvert.DeserializeObject<DonationModel>(donationJson);
            donation.PickUpOn = model.PickUpDateOn.AddHours(model.PickUpTimeOn.Hour).AddMinutes(model.PickUpTimeOn.Hour);
            
            // Create list of categories beeing in relation to the donation
            var categoryIds = new List<int>();
            model.Categories.Where(x => x.IsChecked == true).ToList().ForEach(c => categoryIds.Add(c.Id));
            
            // Find authenticated user Id. Add relation Donation-User
            //string userId;
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.FindByIdAsync(userId).Result;
                donation.User = user;
            }

            var createDonationTask = _donationService.CreateDonationAsync(donation, model.InstitutionId, categoryIds);
            
            // Send confirmation email to authenticated user
            await createDonationTask;
            if (createDonationTask.IsCompletedSuccessfully)
            {
                if (User.Identity.IsAuthenticated)
                {
                    _ = _emailService.SendDonationConfirmation(donation);
                }
                return View();
            }
            return RedirectToAction(nameof(Home.Index), nameof(Home));
        }

        [Authorize]
        public IActionResult GetDonations()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var donations = _donationService.GetDonations(userId);
            return View(donations);
        }

        [NonAction]
        // This method validates model against requirements additional to these set by attributes.
        // If validation fails, add message to ModelState dictionary
        private bool IsModelValid(DonationViewModel model, out List<string> errors)
        {
            errors = new List<string>();
            if (model.Categories.Where(c => c.IsChecked).Count() == 0)
            {
                errors.Add("Musisz zaznaczyć przynajmniej jedną kategorię");
            }
            
            if (model.PhoneNumber != null)
            {
                if (!IsPhoneNumberValid(model.PhoneNumber))
                {
                    errors.Add("Sprawdź numer telefonu. Dozwolone znaki: 0-9, '+- .'. Przykład: 0048.123 456 789");
                }
            }

            if (!(model.PickUpDateOn > new DateTime(
                year: DateTime.Now.Year, 
                month: DateTime.Now.Month, 
                day: DateTime.Now.Day + 2)))
            {
                errors.Add("Na zorganizowanie odbioru musimy mieć przynajmniej 3 dni. Wyznacz termin za 3 dni lub późniejszy");
            }
            return errors.Count == 0;
        }

        [NonAction]
        private bool IsPhoneNumberValid(string numberToCheck)
        {
            Regex regex = new Regex(pattern: @"^((00|\+)[1-9]\d)?[\- \.]?[1-9]\d{2}[\- \.]?\d{3}[\- \.]?\d{3}$");
            return regex.IsMatch(numberToCheck);
        }

        #region Ctor & DI
        private readonly IDonationService _donationService;
        private readonly UserManager<CharityUser> _userManager;
        private readonly ILogger _logger;
        private readonly ICharityEmailService _emailService;
        public Donating(IDonationService donationService, ILoggerFactory loggerFactory, UserManager<CharityUser> userManager, ICharityEmailService emailService)
        {
            _donationService = donationService;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger("DonationController");
            _emailService = emailService;
        }
        #endregion
    }
}
