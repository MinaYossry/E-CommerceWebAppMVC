using System.ComponentModel;

namespace FinalProjectMVC.Areas.SellerPanel.ViewModel
{
    public class DeleteSellerProductViewModel
    {
        [ReadOnly(true)]
        public int SellerProductId { get; set; }

        [ReadOnly(true)]
        public int SerialNumber { get; set; }

        [ReadOnly(true)]
        public required string Name { get; set; }

        [ReadOnly(true)]
        public string? Description { get; set; }

        [ReadOnly(true)]
        public byte[]? ProductImage { get; set; }

        [ReadOnly(true)]
        public int Count { get; set; }

        [ReadOnly(true)]
        public decimal Price { get; set; }
    }
}
