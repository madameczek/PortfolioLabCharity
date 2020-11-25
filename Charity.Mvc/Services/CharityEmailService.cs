using Charity.Mvc.Models.DbModels;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class CharityEmailService : ICharityEmailService
    {
        public Task SendDonationConfirmation(DonationModel donation)
        {
            _emailService.SendAsync(
                donation.User.Email,
                $"{donation.User.Name}, dziękujemy za darowiznę",
                $"<h3>Witaj {donation.User.Name}<h3><br /> Przyjęliśmy donację na: {donation.Quantity} worki.<br />" +
                $"Przygotuj paczkę do odbioru przez kuriera {donation.PickUpOn.ToString("yyyy.MM.dd")} około {donation.PickUpOn.ToString("HH:00")}.<br />" +
                $"Dziękujemy.",
                true);
            return Task.CompletedTask;
        }

        public Task SendEmailConfirmation(string confirmationLink, CharityUser user)
        {
            _emailService.SendAsync(
                user.Email,
                //"marek@adameczek.pl",
                $"{user.Name}, potwierdź rejestrację",
                $"<h3>Kliknij link, by potwierdzić rejestrację do serwisu 'Charity'</h3><br />" +
                $"<a href=\"{confirmationLink}\">Potwierdź adres email</a><br />" +
                $"Zignoruj tę wiadomość, jeśli nie donowywałeś(aś) rejestracji.",
                true);
            return Task.CompletedTask;
        }

        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        public CharityEmailService(IEmailService emailService, ILoggerFactory loggerFactory)
        {
            _emailService = emailService;
            _logger = loggerFactory.CreateLogger("EmailService");
        }
    }
}
