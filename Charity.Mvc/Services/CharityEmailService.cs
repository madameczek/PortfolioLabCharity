using Charity.Mvc.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class CharityEmailService : ICharityEmailService
    {
        public Task SendDonationConfirmation(DonationModel donation)
        {
            _emailService.SendAsync(
                donation.User.Email,
                "Donation Confirmation",
                $"<h3>Witaj {donation.User.Name}<h3><br /> Przyjęliśmy donację na: {donation.Quantity} worki.",
                true);
            return Task.CompletedTask;
        }

        public Task SendEmailConfirmation(CharityUser user)
        {
            throw new NotImplementedException();
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
