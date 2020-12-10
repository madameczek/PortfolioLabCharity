using Charity.Mvc.Contexts;
using Charity.Mvc.Models.DbModels;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class UserManagerService : IUserManagerService
    {
        public bool IsEmailUnique(string email) =>
            _context.Users.FirstOrDefault(u => u.NormalizedEmail == email.ToUpper()) == null;
        
        private readonly CharityDbContext _context;
        public UserManagerService(CharityDbContext context)
        {
            _context = context;
        }

        public async Task ConfirmEmailAsync(CharityUser user)
        {
            _context.Users.FirstOrDefault(u => u.Email == user.Email).EmailConfirmed = true;
            await _context.SaveChangesAsync();
        }
    }
}
