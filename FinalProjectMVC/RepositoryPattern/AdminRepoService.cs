using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;

namespace FinalProjectMVC.RepositoryPattern
{
    public class AdminRepoService : EFCoreRepo<Admin>
    {
        public AdminRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
