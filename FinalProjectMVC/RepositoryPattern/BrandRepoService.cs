using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class BrandRepoService : EFCoreRepo<Brand>
    {
        public BrandRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
