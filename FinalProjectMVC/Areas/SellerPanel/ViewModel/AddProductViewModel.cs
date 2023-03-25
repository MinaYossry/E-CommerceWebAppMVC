using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace FinalProjectMVC.Areas.SellerPanel.ViewModel
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Must enter unique serial number for the product")]
        [DisplayName("Serial Number")]
        public int SerialNumber { get; set; } 

        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Name can't be less than 5 more that 100 letters")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description can't be less than 10 more that 500 letters")]
        [DisplayName("Product Details")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must choose sub category for the product")]
        [DisplayName("Sub Category")]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Must choose brand for the product")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Must add an image for the product")]
        [DisplayName("Product Image")]
        public IFormFile? formFile { get; set; }

        [HiddenInput]
        public required string SellerID { get; set; }

        [Required(ErrorMessage = "Must enter price for your product")]
        [Range(0, 100000, ErrorMessage = "Price can't be negative")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Must enter stock for your product")]
        [Range(0, 100000, ErrorMessage = "Count can't be negative")]
        public int Count { get; set; }

        public SelectList? Brands { get; set; }

        public SelectList? SubCategories { get; set; }
    }
}
