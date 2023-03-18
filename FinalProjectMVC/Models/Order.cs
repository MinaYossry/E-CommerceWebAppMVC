using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FinalProjectMVC.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        //Forignkey
        //public int CustomerId { get; set; }
        //public Customer customer { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
    }
}
