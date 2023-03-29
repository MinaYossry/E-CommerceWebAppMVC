using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.ViewModels
{
    public class AddReviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey(nameof(Seller))]
        public required string SellerId { get; set; }
        public virtual Seller? Seller { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
       
    }
}
