using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.CustomerPanel.ViewModel
{
    [NotMapped]
    public class CustomerOrderItemViewModel
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public int SellerProductId { get; set; }

        public int Count { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public required Seller Seller { get; set; }

        public required Product Product { get; set; }
    }
}
