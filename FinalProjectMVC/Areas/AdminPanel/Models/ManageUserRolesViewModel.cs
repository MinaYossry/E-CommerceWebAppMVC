using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectMVC.Areas.AdminPanel.Models
{
    public class ManageUserRolesViewModel 
    {

        public List<ApplicationUser>? Users { get; set; }
        public List<IdentityRole>? Roles { get; set; }
        public string? SelectedUserId { get; set; }
        public string? SelectedRoleId { get; set; }

    }
}
