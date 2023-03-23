using FinalProjectMVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Customer
    {
        [ForeignKey("ApplicationUser")]
        public required string Id { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        public virtual List<Address>? Addresses { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Report>? Reports { get; set; } 
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }
}
