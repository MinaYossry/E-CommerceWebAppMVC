

using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Areas.CustomerPanel.ViewModel
{
    public class DisplayInStockProductsViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public byte[]? ProductImage { get; set; }
        public string? SellerNameWithLowestPrice { get; set; }
        public decimal LowestPrice { get; set; }

        public string? SubCategory { get; set; }
            
        public string? Brand { get; set;  }

        public int Count { get; set; }


        public SellerProduct? sellerProductWithLowestPrice { get; set; }
    }
}
