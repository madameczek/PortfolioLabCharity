using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Charity.Mvc.Controllers
{
    [RequireHttps]
    public class Account : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManagerService.GetUserByEmail(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Nie odnaleziono użytkownika lub błędne hasło");
                        return View(model);
                    }

                    await _signInManager.SignOutAsync();
                    var loginResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
                    if (loginResult.Succeeded)
                    {
                        _logger.LogInformation("User {User} logged in to application as {Role} ", user.Email, await _userManager.GetRolesAsync(user));
                        return RedirectToAction(nameof(Index), nameof(Home));
                    }

                    if (loginResult.IsNotAllowed)
                    {
                        _logger.LogWarning("User {User} is not allowed to login.", model.Email);
                        ModelState.AddModelError("", "Konto nie aktywne. Sprawdź skrzynkę email i kliknij link aktywacyjny.");
                        return View(model);
                    }
                    
                    _logger.LogWarning("Invalid attempt to login from email {Email}", model.Email);
                    ModelState.AddModelError("", "Nie odnaleziono użytkownika lub błędne hasło");
                    return View(model);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error logging in user.");
                    return RedirectToAction(nameof(Index), nameof(Home));
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
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    if (!_userManagerService.IsEmailUnique(model.Email))
                    {
                        ModelState.AddModelError("", "Ten adres email jest już zarejestrowany");
                        return View(model);
                    }

                    CharityUser user = new CharityUser()
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                    };
                    user.UserName = $"{user.Name}{user.Surname}".Replace(" ", "_");
                    user.UserName = RemoveDiacritics(user.UserName);
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();

                    // Create user
                    var createUserResult = await _userManager.CreateAsync(user, model.Password);
                    if(createUserResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        _logger.LogInformation("Created user with email {User}.", user.Email);
                        // Prepare token & send it via email
                        _ = GenerateAndSendTokeByEmail(user);
                        return RedirectToAction(nameof(SuccessfulRegistration));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nie udało się dodać użytkownika");
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

        [HttpGet]
        public IActionResult SuccessfulRegistration() 
        { 
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmed(string user, string token)
        {
            var identityUser = _userManager.FindByIdAsync(user).Result;
            if(identityUser != null)
            {
                var result = await _userManager.ConfirmEmailAsync(identityUser, token);
                if (result.Succeeded || await _userManager.IsEmailConfirmedAsync(identityUser))
                {
                    return View(nameof(EmailConfirmed));
                }
                else
                {
                    var enumerator = result.Errors.GetEnumerator();
                    var errorList = new List<string>();
                    while (enumerator.MoveNext())
                    {
                        errorList.Add(enumerator.Current.Description);
                    }
                    if (errorList.Contains("Invalid token."))
                    {
                        _logger.LogError("Confirming Email error. Error list: {Error}", errorList);
                        _ = GenerateAndSendTokeByEmail(identityUser);
                        ViewBag.Message = "Twój link aktywacyjny wygasł.";
                        return View(nameof(SuccessfulRegistration));
                    }
                    ViewBag.Errors = errorList;
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }
            return RedirectToAction(nameof(Error));
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var temp = JsonConvert.SerializeObject(user, settings: new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var newUser = JsonConvert.DeserializeObject<EditProfileViewModel>(temp);
            return View(newUser);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PhoneNumber != null && !IsPhoneNumberValid(model.PhoneNumber))
                {
                    ModelState.AddModelError("", "Sprawdź ne telefonu. Może zawierać znaki: 0-9, '+- .'. np: 0048.501-502-503.");
                    return View(model);
                }

                try
                {
                    var identityUser = await _userManager.GetUserAsync(User);

                    identityUser.Name = model.Name;
                    identityUser.Surname = model.Surname;
                    identityUser.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(identityUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Home.Index), nameof(Home));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error updating user {User}", model.Email);
                    throw;
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Home.Index), nameof(Home));
        }

        public IActionResult Error(List<string> errors)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            normalizedString = normalizedString.Replace('ł', 'l');
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

        [NonAction]
        private async Task GenerateAndSendTokeByEmail(CharityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(EmailConfirmed), nameof(Account), new { token, user = user.Id }, Request.Scheme, Request.Host.ToString());
            _ = _emailService.SendEmailConfirmation(confirmationLink, user);
            return;
        }

        [NonAction]
        private bool IsPhoneNumberValid(string numberToCheck)
        {
            Regex regex = new Regex(pattern: @"^((00|\+)48)?[\- \.]?[1-9]\d{2}[\- \.]?\d{3}[\- \.]?\d{3}$");
            return regex.IsMatch(numberToCheck);
        }

        #region Ctor & DI
        private readonly SignInManager<CharityUser> _signInManager;
        private readonly UserManager<CharityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserManagerService _userManagerService;
        private readonly ICharityEmailService _emailService;
        private readonly ILogger _logger;
        public Account(
            ILoggerFactory loggerFactory,
            SignInManager<CharityUser> signInManager, 
            UserManager<CharityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserManagerService userManagerService,
            ICharityEmailService emailService)
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
