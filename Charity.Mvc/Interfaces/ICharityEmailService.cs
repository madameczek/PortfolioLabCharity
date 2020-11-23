using Charity.Mvc.Models.DbModels;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public interface ICharityEmailService
    {
        public Task SendDonationConfirmation(DonationModel donation);
        public Task SendEmailConfirmation(CharityUser user);
    }
}