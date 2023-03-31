using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.ViewModels
{
    public class AddReviewViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public required string CustomerId { get; set; }

        public required string SellerId { get; set; }

        public int ProductId { get; set; }
    }
}
