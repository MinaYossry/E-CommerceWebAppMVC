using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        //public int SallerId { get; set; }
        //public  Saller saller { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
