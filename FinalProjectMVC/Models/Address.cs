using FinalProjectMVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public required string StreetName { get; set; }

        public required string BuildingNumber { get; set; }

        public required string City { get; set; }

        public required string Region { get; set; }

        [ForeignKey("ApplicationUser")]
        public required string UserId { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
