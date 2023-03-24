using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Models
{
    public class Report
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool IsSolved { get; set; } = false;

        public DateTime CreatedDate { get; } = DateTime.Now;

        [ForeignKey(nameof(Review))]
        public int ReviewId { get; set; }

        public Review? Review { get; set; }
    }
}
