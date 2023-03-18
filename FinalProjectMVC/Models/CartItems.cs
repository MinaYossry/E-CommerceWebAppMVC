using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }

        public Cart? Cart { get; set; }

        //public int SallerId { get; set; }
        //public int ProductId { get; set; }
        //public  Saller saller { get; set; }
        //public Product product { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
