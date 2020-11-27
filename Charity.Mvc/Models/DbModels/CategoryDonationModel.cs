using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.Mvc.Models.DbModels
{
    public class CategoryDonationModel
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public int CategoryId { get; set; }

        // Relationships
        [ForeignKey("DonationId")]
        public virtual DonationModel Donation { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category { get; set; }
    }
}
