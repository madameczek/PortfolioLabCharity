using Charity.Mvc.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public interface IUserManagerService
    {
        public bool IsEmailUnique(string email);

        public CharityUser GetUserByEmail(string email);

        public string GetFirstNameById(string userId);
    }
}
