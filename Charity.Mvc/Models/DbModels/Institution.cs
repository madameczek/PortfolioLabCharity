using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.DbModels
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desctiption { get; set; }

        [ForeignKey("Donation")]
        public int DonationId { get; set; }
    }
}
