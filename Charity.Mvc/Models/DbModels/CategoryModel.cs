using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.DbModels
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        // Relationships
        public virtual ICollection<CategoryDonationModel> CategoryDonation { get; set; }
    }
}
