using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        //Forignkey
        //public int CustomerId { get; set; }
        //public Customer customer { get; set; }
    }
}
