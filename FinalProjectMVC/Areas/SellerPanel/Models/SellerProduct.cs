using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.SellerPanel.Models
{
    public class SellerProduct
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Seller))]
        public int SellerId { get; set; }
        public virtual Seller? Seller { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
