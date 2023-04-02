using FinalProjectMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.SellerPanel.Models
{
    public class SellerProduct
    {
        public int Id { get; set; }

        public int Count { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Seller))]
        public required string SellerId { get; set; }
        public virtual Seller? Seller { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }

        [NotMapped]
        public string DataTextFieldLabel
        {
            get
            {
                return $"{Seller?.ApplicationUser?.FirstName} | ${Math.Round(Price, 2)}";
            }
        }
    }
}
