using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace FinalProjectMVC.Models
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(order))]
        public int OrderId { get; set; }

        public Order order { get; set; }

        //public int SallerId { get; set; }
        //public int ProductId { get; set; }
        //public  Saller saller { get; set; }
        //public Product product { get; set; }

        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Price { get; set; }

    }
}
