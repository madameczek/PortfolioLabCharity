using Charity.Mvc.Models.DbModels;
using System.Threading.Tasks;
using Charity.Mvc.Models.ViewModels;

namespace Charity.Mvc.Services
{
    public interface ICharityEmailService
    {
        public Task SendDonationConfirmation(DonationModel donation);
        public Task SendRegistrationConfirmation(string confirmationLink, CharityUser user);
        public Task SendResetPasswordConfirmation(string confirmationLink, CharityUser user);
        public  Task SendContactFormMessage(ContactFormViewModel model);
    }
}