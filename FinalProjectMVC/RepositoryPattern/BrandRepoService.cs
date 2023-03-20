using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class BrandRepoService : EFCoreRepo<Brand>
    {
        public BrandRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
