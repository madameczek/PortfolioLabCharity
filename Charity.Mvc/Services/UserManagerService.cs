using Charity.Mvc.Contexts;
using Charity.Mvc.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class UserManagerService : IUserManagerService
    {

        public bool IsEmailUnique(string email)
        {
            return _context.Users.Where(u => u.NormalizedEmail == email.ToUpper()).FirstOrDefault() == null;
        }

        public CharityUser GetUserByEmail(string email)
        {
            return _context.Users.Where(u => u.NormalizedEmail == email.ToUpper()).FirstOrDefault();
        }

        private readonly CharityDbContext _context;
        public UserManagerService(CharityDbContext context)
        {
            _context = context;
        }
    }
}
