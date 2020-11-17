using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.Mvc.Models.DbModels
{
    public class Institution
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey("Donation")]
        public int DonationId { get; set; }
    }
}
