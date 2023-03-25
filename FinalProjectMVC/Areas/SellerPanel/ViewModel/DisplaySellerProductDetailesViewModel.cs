using FinalProjectMVC.Areas.SellerPanel.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Areas.SellerPanel.ViewModel
{
    public class DisplaySellerProductDetailesViewModel
    {
        public int SellerProductId { get; set; }

        public int SerialNumber { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public byte[]? ProductImage { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
