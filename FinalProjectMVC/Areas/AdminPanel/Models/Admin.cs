using FinalProjectMVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.AdminPanel.Models
{
    public class Admin
    {
        [ForeignKey("ApplicationUser")]
        public required string Id { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
