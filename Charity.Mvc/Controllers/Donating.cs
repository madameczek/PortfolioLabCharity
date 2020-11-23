using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.Mvc.Controllers
{
    [RequireHttps]
    public class Donating : Controller
    {
        
        private List<string> errors;

        private readonly IDonationService _donationService;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        public Donating(IDonationService donationService, ILoggerFactory loggerFactory, IEmailService emailService)
        {
            _donationService = donationService;
            _logger = loggerFactory.CreateLogger("DonationController");
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Donate()
        {
            try
            {
                var categories = JsonConvert.SerializeObject(_donationService.GetCategoryList());
                var institutions = JsonConvert.SerializeObject(_donationService.GetInstitutionList(take: 0));
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
            var donationJson = JsonConvert.SerializeObject(model);
            var donation = JsonConvert.DeserializeObject<DonationModel>(donationJson);
            donation.PickUpOn = model.PickUpDateOn.AddHours(model.PickUpTimeOn.Hour).AddMinutes(model.PickUpTimeOn.Hour);
            // create list of categories beeing in relation to the donation
            var categoryIds = new List<int>();
            model.Categories.Where(x=>x.IsChecked==true).ToList().ForEach(c => categoryIds.Add(c.Id));
            await _donationService.CreateDonationAsync(donation, model.InstitutionId, categoryIds);

            return View();
        }

        [NonAction]
        private bool IsModelValid(DonationViewModel model, out List<string> errors)
        {
            errors = new List<string>();
            if (model.Categories.Where(c => c.IsChecked).Count() == 0)
            {
                errors.Add("Musisz zaznaczyć przynajmniej jedną kategorię");
            }
            
            if (model.PhoneNumber != null)
            {
                if (!model.PhoneNumber.All(c => char.IsDigit(c) || c == ' ' || c == '-'))
                {
                    errors.Add("Sprawdź numer telefonu. Może zawierać tylko cyfry, spacje i znak '-'. Znak + zamień na dwa zera.");
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

        private void SendEmailAsync()
        {

        }
    }
}
