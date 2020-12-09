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
        public bool IsEmailUnique(string email) =>
            _context.Users.FirstOrDefault(u => u.NormalizedEmail == email.ToUpper()) == null;
        
        private readonly CharityDbContext _context;
        public UserManagerService(CharityDbContext context)
        {
            _context = context;
        }
    }
}
