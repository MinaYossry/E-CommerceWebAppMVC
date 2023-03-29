using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Models
{
    public class Review
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [ReadOnly(true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey(nameof(Seller))]
        public required string SellerId { get; set; }
        public virtual Seller? Seller { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public virtual ICollection<Report>? Reports { get; set; }
    }
}
