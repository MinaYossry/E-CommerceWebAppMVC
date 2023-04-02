using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.ViewModels
{
    public class FrontPageViewModel
    {
        public required List<Product> FeaturedProducts { get; set; }
        public required List<Product> BestSelllerProducts { get; set; }
    }
}