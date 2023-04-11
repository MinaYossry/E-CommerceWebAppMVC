using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CategoryRepoService : EFCoreRepo<Category>
    {
        public CategoryRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
