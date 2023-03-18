using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(01[0-2]|010|011|012|015)[0-9]{8}$")]
        public string? Phone { get; set; }


        public string? Address { get; set; }
    }
}
