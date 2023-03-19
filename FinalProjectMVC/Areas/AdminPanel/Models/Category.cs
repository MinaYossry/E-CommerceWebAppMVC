using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.Areas.AdminPanel.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // Navigation property for Products
        public virtual ICollection<SubCategory>? SubCategories { get; set; }
    }
}
