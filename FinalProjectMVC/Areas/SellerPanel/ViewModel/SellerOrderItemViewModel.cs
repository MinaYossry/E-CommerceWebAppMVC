
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.SellerPanel.ViewModel
{
    [NotMapped]
    public class SellerOrderItemViewModel
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public int Count { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public required Product Product { get; set; }

        public required Customer Customer { get; set; }

        public required Address Address { get; set; }
    }
}
