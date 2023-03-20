using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CategoryRepoService : EFCoreRepo<Category>
    {
        public CategoryRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
