using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SubCategoryRepoService : EFCoreRepo<SubCategory>
    {
        public SubCategoryRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
