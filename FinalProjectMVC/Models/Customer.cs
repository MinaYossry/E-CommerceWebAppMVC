using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Customer : Person
    {
        [DataType(DataType.Currency)]
        public decimal? Balance { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Report>? Reports { get; set; } 
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
