using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Charity.Mvc.Models;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.Extensions.Logging;

namespace Charity.Mvc.Controllers
{
	public class HomeController : Controller
	{
		private readonly IDonationService _donationService;
		private readonly ILogger _logger;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="donationService"></param>
		/// <param name="loggerFactory"></param>
        public HomeController(IDonationService donationService, ILoggerFactory loggerFactory)
        {
            _donationService = donationService;
            _logger = loggerFactory.CreateLogger("Home Controller");
        }

        public IActionResult Index()
		{
			OrganisationsBagsViewModel model;
			try
			{
				model = new OrganisationsBagsViewModel()
				{
					Institutions = _donationService.GetInstitutionList(),
					InstitutionCount = _donationService.GetInstitutionCount(),
					BagCount = _donationService.GetTotalNumberOfBags()
				};
			}
            catch (Exception)
            {
                throw;
            }
			return View(model);
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
