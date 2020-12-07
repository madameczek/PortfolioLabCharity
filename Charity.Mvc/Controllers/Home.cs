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
        public Home(IDonationService donationService, ILoggerFactory loggerFactory)
        {
            _donationService = donationService;
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

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
