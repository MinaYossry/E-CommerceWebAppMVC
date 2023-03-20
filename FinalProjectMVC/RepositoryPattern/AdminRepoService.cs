using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class AdminRepoService : EFCoreRepo<Admin>
    {
        public AdminRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
