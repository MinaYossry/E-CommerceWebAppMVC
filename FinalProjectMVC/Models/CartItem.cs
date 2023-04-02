using FinalProjectMVC.Areas.SellerPanel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey("SellerProduct")]
        public int SellerProductId { get; set; }

        public virtual SellerProduct? SellerProduct { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
