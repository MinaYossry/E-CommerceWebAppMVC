using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.AdminPanel.Models;

namespace FinalProjectMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int SerialNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? Folder { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [ForeignKey(nameof(SubCategory))]
        public int SubCategoryId { get; set; }

        // Navigation property for Category
        // Marked as virtual to enable lazy loading
        public virtual SubCategory? SubCategory { get; set; }

        [Required]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        // Navigation property for Brand
        // Marked as virtual to enable lazy loading
        public virtual Brand? Brand { get; set; }


        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
    }
}
