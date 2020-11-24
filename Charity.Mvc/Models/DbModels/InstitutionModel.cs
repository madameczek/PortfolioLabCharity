using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.DbModels
{
    public class InstitutionModel
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        // Relationships
        public ICollection<DonationModel> Donations { get; set; }
    }
}
