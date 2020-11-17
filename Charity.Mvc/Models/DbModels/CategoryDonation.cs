using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.Mvc.Models.DbModels
{
    public class CategoryDonation
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("DonationId")]
        public Donation Donation { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
