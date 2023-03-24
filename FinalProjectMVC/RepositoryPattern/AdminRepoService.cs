using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class AdminRepoService : EFCoreRepo<Admin>
    {
        public AdminRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
