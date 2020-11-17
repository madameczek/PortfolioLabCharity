using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
