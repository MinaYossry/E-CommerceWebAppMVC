using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class Customer : Person
    {
        [DataType(DataType.Currency)]
        public decimal? Balance { get; set; }
    }
}
