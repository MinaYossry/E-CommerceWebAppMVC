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

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        //public int SallerId { get; set; }
        //public  Saller saller { get; set; }

        [Required]
        [Range(0,10)]
        public int Count { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
}
