using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.Mvc.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManagerService.GetUserByEmail(model.Email);
                    if (user == null)
                    {
                        TempData["warning_email"] = "Nie odnaleziono adresu e-mail";
                        return View(model);
                    }

                    await _signInManager.SignOutAsync();
                    var loginResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
                    if (loginResult.Succeeded)
                    {
                        _ = _emailService.SendAsync("marek@adameczek.pl", "Login confirmation", "Logowanie do aplikacji Charity");
                        return RedirectToAction("Index", "Home");
                    }

                    TempData["warning_password"] = "Pokombinuj jeszcze z hasłem";
                    return View(model);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error logging in user.");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationAsync(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    if (!_userManagerService.IsEmailUnique(model.Email))
                    {
                        TempData["warning_email"] = "Ten adres email jest już zarejestrowany";
                        return View(model);
                    }

                    CharityUser user = new CharityUser()
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                    };
                    user.UserName = $"{user.Name}_{user.Surname}".Replace(" ", "_");
                    user.UserName = RemoveDiacritics(user.UserName);
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();

                    var createUserResult = await _userManager.CreateAsync(user, model.Password);
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

                    if(createUserResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        _logger.LogInformation("Created user with email {User}.", user.Email);
                        //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var link = Url.Action(action: nameof(VerifyEmailAsync), controller: nameof(AccountController), new { user.Id, token });
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        _logger.LogWarning("Valid user model with email {User} not created.", user.Email);
                        TempData["warning_regerror"] = "Wystąpił błąd";
                        return View(model);
                    }
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Error creating user.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> VerifyEmailAsync(string userId, string token)
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        #region Ctor & DI
        private readonly SignInManager<CharityUser> _signInManager;
        private readonly UserManager<CharityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserManagerService _userManagerService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        public AccountController(
            ILoggerFactory loggerFactory,
            SignInManager<CharityUser> signInManager, 
            UserManager<CharityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserManagerService userManagerService,
            IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger("Account Controller");
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userManagerService = userManagerService;
            _emailService = emailService;
        }
        #endregion
    }
}
