using Charity.Mvc.Models.DbModels;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public interface ICharityEmailService
    {
        public Task SendDonationConfirmation(DonationModel donation);
        public Task SendEmailConfirmation(string confirmationLink, CharityUser user);
    }
}