using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ProductRepoService : EFCoreRepo<Product>
    {
        public ProductRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
