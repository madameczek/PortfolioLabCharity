using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.DbModels
{
    public class CharityUser : IdentityUser
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }
    }
}
