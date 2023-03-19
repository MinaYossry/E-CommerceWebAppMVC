using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class Seller : Person
    {
        public decimal Balance { get; set; }

        public string? TaxNumber { get; set; }

        [Range (1,5)]
        public int Rating { get; set; }

    }
}
