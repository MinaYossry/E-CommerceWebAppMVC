using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.Areas.SellerPanel.Models
{
    public class Seller : Person
    {
        public decimal Balance { get; set; }

        public string? TaxNumber { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public virtual ICollection<SellerProduct>? SellerProducts { get; set; }
    }
}
