using NETCore.MailKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class CharityEmailService : EmailService, IEmailService
    {
        private string DonationCreatedConfirmation;
        public CharityEmailService(IMailKitProvider provider) : base(provider)
        {
        }

        public Task SendAsync()
        {
            throw new NotImplementedException();
        }
    }
}
