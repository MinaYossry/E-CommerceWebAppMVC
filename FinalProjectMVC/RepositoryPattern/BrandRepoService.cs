using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class BrandRepoService : EFCoreRepo<Brand>
    {
        public BrandRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
