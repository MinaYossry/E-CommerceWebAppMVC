using FinalProjectMVC.Areas.SellerPanel.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        OutForDelivery,
        Delivered,
        Cancelled
    }
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue(OrderStatus.Pending)]
        [EnumDataType(enumType: typeof(OrderStatus))]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        [ForeignKey("SellerProduct")]
        public int SellerProductId { get; set; }

        public virtual SellerProduct? SellerProduct { get; set; }

        [Required]
        [Range(0, 10)]
        public int Count { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}