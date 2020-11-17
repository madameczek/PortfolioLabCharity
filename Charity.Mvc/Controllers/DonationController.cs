using Charity.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Controllers
{
    public class DonationController : Controller
    {
        [HttpGet]
        public IActionResult Donate()
        {
            return View(nameof(Donate));
        }

        [HttpPost]
        public IActionResult Donate(int model)
        {

            return View();
        }
    }
}
