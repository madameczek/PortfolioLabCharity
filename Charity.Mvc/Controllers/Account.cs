using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
            if (!ModelState.IsValid) return View(model);
            if (model.Password == null)
            {
                ModelState.AddModelError("", "Podaj hasło");
                return View(model);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
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

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                if (!_userManagerService.IsEmailUnique(model.Email))
                {
                    ModelState.AddModelError("", "Ten adres email jest już zarejestrowany");
                    return View(model);
                }

                var user = new CharityUser()
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
                if (createUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("Created user with email {User}.", user.Email);
                    // Prepare token & send it via email
                    _ = GenerateAndSendEmailConfirmationToken(user);
                    return RedirectToAction(nameof(ConfirmRegistration));
                }

                ModelState.AddModelError("", "Nie udało się dodać użytkownika");
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating user.");
            }

            return View(model);
        }

        [HttpGet]
        // Show a page with 'click emailed link' prompt
        public IActionResult ConfirmRegistration() 
        { 
            return View(); 
        }

        [HttpGet]
        // Invoked by emailed link with token
        public async Task<IActionResult> RegistrationConfirmed(string user, string token)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(token)) return RedirectToAction(nameof(Error));
            var identityUser = await _userManager.FindByIdAsync(user);
            if (identityUser == null) return RedirectToAction(nameof(Error));

            var result = await _userManager.ConfirmEmailAsync(identityUser, token);
            if (result.Succeeded || await _userManager.IsEmailConfirmedAsync(identityUser))
            {
                return View(nameof(RegistrationConfirmed));
            }

            // If errors occured while confirming token, build error list
            using var enumerator = result.Errors.GetEnumerator();
            var errorList = new List<string>();
            while (enumerator.MoveNext())
            {
                errorList.Add(enumerator.Current.Description);
            }
            
            if (errorList.Contains("Invalid token."))
            {
                _logger.LogError("Confirming Email error. Error list: {Error}", errorList);
                _ = GenerateAndSendEmailConfirmationToken(identityUser);
                ViewBag.Message = "Twój link aktywacyjny wygasł.";
                return View(nameof(ConfirmRegistration));
            }

            ViewBag.Errors = errorList;
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var temp = JsonConvert.SerializeObject(
                user,
                settings: new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            var newUser = JsonConvert.DeserializeObject<EditProfileViewModel>(temp);
            return View(newUser);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!string.IsNullOrEmpty(model.PhoneNumber) && !IsPhoneNumberValid(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Sprawdź ne telefonu. Może zawierać znaki: 0-9, '+- .'. np: 0048.501-502-503.");
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.PhoneNumber;
                // Attempt to change user data
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Nie udało się zmienić danych");
                    return View(model);
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (!string.IsNullOrEmpty(model.CurrentPassword))
                    {
                        var passChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                        if (passChangeResult.Succeeded) return View("PasswordChangeConfirmed");

                        ModelState.AddModelError("", "Nie udało się zmienić hasła.");
                        passChangeResult.Errors
                            .Select(identityError => identityError.Description)
                            .ToList()
                            .ForEach(errorMessage => ModelState.AddModelError("", errorMessage));
                        return View(model);
                    }
                    ModelState.AddModelError("", "Aby zmienić hasło, musisz podać stare i nowe hasła.");
                    return View(model);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Home.Index), nameof(Home));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating user {User}", model.Email);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Show a page with 'click emailed link to reset password' prompt
        public async Task<IActionResult> ConfirmResetPassword(LoginViewModel model)
        {
            if (ModelState.GetFieldValidationState("Email") == ModelValidationState.Invalid)
            {
                ModelState.AddModelError("", "Podaj adres email");
                return View(nameof(Login), model);
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    //ModelState.AddModelError("", "Nie odnaleziono adresu email");
                    return View(model);
                }
                // Prepare token & send it via email
                _ = GenerateAndSendPasswordResetToken(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }
            return View();
        }

        [HttpGet]
        // Method invoked by email link
        // Show to a user a page with reset password form
        public async Task<IActionResult> ResetPassword(string user, string token)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(token)) return RedirectToAction(nameof(Error));
            var identityUser = await _userManager.FindByIdAsync(user);
            if (identityUser == null) return RedirectToAction(nameof(Error)); // or redirect to login page
            var model = new ResetPasswordViewModel() { Email = identityUser.Email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) return View("PasswordChangeConfirmed");

            ModelState.AddModelError("", "Nie udało się zmienić hasła.");
            result.Errors
                .Select(identityError => identityError.Description)
                .ToList()
                .ForEach(errorMessage => ModelState.AddModelError("", errorMessage));
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
        private async Task GenerateAndSendPasswordResetToken(CharityUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(ResetPassword), 
                nameof(Account), 
                new { token, user = user.Id }, 
                Request.Scheme, 
                Request.Host.ToString());
            _ = _emailService.SendResetPasswordConfirmation(confirmationLink, user);
        }

        [NonAction]
        private async Task GenerateAndSendEmailConfirmationToken(CharityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(RegistrationConfirmed), 
                nameof(Account), 
                new { token, user = user.Id }, 
                Request.Scheme, 
                Request.Host.ToString());
            _ = _emailService.SendRegistrationConfirmation(confirmationLink, user);
        }
        
        [NonAction]
        private static string RemoveDiacritics(string text)
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
        private static bool IsPhoneNumberValid(string numberToCheck)
        {
            var regex = new Regex(pattern: @"^((00|\+)48)?[\- \.]?[1-9]\d{2}[\- \.]?\d{3}[\- \.]?\d{3}$");
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
