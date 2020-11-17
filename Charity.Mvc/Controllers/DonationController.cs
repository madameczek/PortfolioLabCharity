using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly ILogger _logger;
        public DonationController(IDonationService donationService, ILoggerFactory loggerFactory)
        {
            _donationService = donationService;
            _logger = loggerFactory.CreateLogger("Donation Controller");
        }

        [HttpGet]
        public IActionResult Donate()
        {
            var model = new DonationViewModel()
            {
                Institutions = _donationService.GetInstitutionList(take: 0),
                Categories = _donationService.GetCategoryList()
            };

            return View(nameof(Donate), model);
        }

        [HttpPost]
        public IActionResult Donate(int model)
        {

            return View();
        }
    }
}
