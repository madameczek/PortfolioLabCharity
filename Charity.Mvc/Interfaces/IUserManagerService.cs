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

        public Task ConfirmEmailAsync(CharityUser user);
    }
}
