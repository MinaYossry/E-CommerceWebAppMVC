using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // Navigation property for Products
        public virtual ICollection<Product>? Products { get; set; }
    }
}
