using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ProductRepoService : EFCoreRepo<Product>
    {
        public ProductRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
