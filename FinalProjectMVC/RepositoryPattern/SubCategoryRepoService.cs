using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SubCategoryRepoService : EFCoreRepo<SubCategory>
    {
        public SubCategoryRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
