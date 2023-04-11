using FinalProjectMVC.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Report
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool IsSolved { get; set; } = false;

        [ReadOnly(true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? SolveDate { get; set; }

        [ForeignKey(nameof(Review))]
        public int ReviewId { get; set; }

        public virtual Review? Review { get; set; }

        [ForeignKey("ApplicationUser")]
        public required string ApplicationUserId { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
