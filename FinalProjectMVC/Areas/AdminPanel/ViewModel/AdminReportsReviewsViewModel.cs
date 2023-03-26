using FinalProjectMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FinalProjectMVC.Areas.AdminPanel.ViewModel
{
    public class AdminReportsReviewsViewModel
    {
        public int ReportId { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool IsSolved { get; set; }

        public DateTime? SolveDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int ReviewId { get; set; }

        public required string ReviewName { get; set; }

        public required string ReviewDescription { get; set; }

        public required string SellerId { get; set; }

        public required string SellerName { get; set; }

        public required string CustomerId { get; set; }

        public required string CustomerName { get; set;}
    }
}
