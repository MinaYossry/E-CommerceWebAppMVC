using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CategoryRepoService : EFCoreRepo<Category>
    {
        public CategoryRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
