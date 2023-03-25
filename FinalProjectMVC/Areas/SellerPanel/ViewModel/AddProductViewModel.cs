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
        public int SerialNumber { get; set; } 

        [StringLength(100, ErrorMessage = "Name can't be more that 100 letters")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can't be more that 500 letters")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must choose sub category for the product")]
        [DisplayName("Sub Category")]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Must choose brand for the product")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [DisplayName("Product Image")]
        public IFormFile formFile { get; set; }

        [HiddenInput]
        public required string SellerID { get; set; }

        public int Price { get; set; }

        public int Count { get; set; }

        public SelectList? Brands { get; set; }

        public SelectList? SubCategories { get; set; }
    }
}
