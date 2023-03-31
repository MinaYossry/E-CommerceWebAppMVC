using FinalProjectMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.CustomerPanel.ViewModel
{
    [NotMapped]
    public class CustomerOrdersViewModel
    {
        public int OrderId { get; set; }

        public required string CustomerId { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public required List<CustomerOrderItemViewModel> OrderItems { get; set; }

        public required Address Address { get; set; }
    }
}
