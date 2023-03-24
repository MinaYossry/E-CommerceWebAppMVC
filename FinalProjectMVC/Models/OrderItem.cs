using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        [ForeignKey("SellerProduct")]
        public int SellerProductId { get; set; }

        public virtual SellerProduct? SellerProduct { get; set; }

        [Required]
        [Range(0,10)]
        public int Count { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

    }
}
