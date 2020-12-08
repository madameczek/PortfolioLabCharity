using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Charity.Mvc.Controllers
{
    public class Home : Controller
	{
		private readonly IDonationService _donationService;
		private readonly ILogger _logger;
        private readonly ICharityEmailService _emailService;
        public Home(IDonationService donationService, ICharityEmailService emailService, ILoggerFactory loggerFactory)
        {
            _donationService = donationService;
            _emailService = emailService;
            _logger = loggerFactory.CreateLogger("Home Controller");
        }

		[HttpGet]
        public IActionResult Index()
		{
			OrganisationsBagsViewModel model = null;
			try
			{
				model = new OrganisationsBagsViewModel()
				{
					Institutions = _donationService.GetInstitutionList(),
					InstitutionCount = _donationService.GetInstitutionCount(),
					BagCount = _donationService.GetTotalNumberOfBags()
				};
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing Index view.");
            }
			return View(model);
		}

        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult ContactFormMessage(ContactFormViewModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName) || 
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.Message))
            {
                return RedirectToAction(nameof(Index));
            }
            _ = _emailService.SendContactFormMessage(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
