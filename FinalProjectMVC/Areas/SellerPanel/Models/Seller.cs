using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.Areas.SellerPanel.Models
{
    public class Seller
    {

        [ForeignKey("ApplicationUser")]
        public required string Id { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Balance { get; set; } = 0;

        public string? TaxNumber { get; set; }


        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<SellerProduct>? SellerProducts { get; set; }
        public virtual List<Address>? Addresses { get; set; }
    }
}
