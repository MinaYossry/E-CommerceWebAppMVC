

using FinalProjectMVC.Areas.SellerPanel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProjectMVC.Areas.CustomerPanel.ViewModel
{
    public class DetailedProductViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public byte[]? ProductImage { get; set; }
        public string? SellerName { get; set; } // SellerName
        //public decimal LowestPrice { get; set; }

        public string? SellerId { get; set; } // note that : SellerID is a `String`

        public string? SubCategory { get; set; }
            
        public string? Brand { get; set;  }

        public int Count { get; set; }


        public SellerProduct? CurrentSellerProduct { get; set; } // Seller record


        //public IEnumerable<SelectListItem>? SellersList; 
        public List<SellerProduct>? SellersList; 

    }
}
