using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class Product
    {
        [Key]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Image { get; set; }

        public string Folder { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation property for Category
        // Marked as virtual to enable lazy loading
        public virtual Category Category { get; set; }

        [Required]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        // Navigation property for Brand
        // Marked as virtual to enable lazy loading
        public virtual Brand Brand { get; set; }
    }
}
