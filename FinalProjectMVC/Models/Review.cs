using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Review
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        [Range(1,5)]
        public int Rating { get; set; }

        public DateTime CreatedDate { get; } = DateTime.Now;

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        // public virtual Seller Seller { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
