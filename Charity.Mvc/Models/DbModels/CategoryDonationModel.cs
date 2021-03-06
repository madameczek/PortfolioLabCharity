﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.Mvc.Models.DbModels
{
    public class CategoryDonationModel
    {
        [Key]
        public int Id { get; set; }
        public int DonationId { get; set; }
        public int CategoryId { get; set; }

        // Relationships
        [ForeignKey("DonationId")]
        public DonationModel Donation { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
    }
}
