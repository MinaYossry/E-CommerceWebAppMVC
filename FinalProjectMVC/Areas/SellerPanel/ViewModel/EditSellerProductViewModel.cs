using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Areas.SellerPanel.ViewModel
{
    public class EditSellerProductViewModel
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

        [Required(ErrorMessage = "Must enter price for your product")]
        [Range(0, 100000, ErrorMessage = "Price can't be negative")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Must enter stock for your product")]
        [Range(0, 100000, ErrorMessage = "Count can't be negative")]
        public int Count { get; set; }
    }
}
