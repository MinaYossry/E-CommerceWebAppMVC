using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SellerProductRepoService : EFCoreRepo<SellerProduct>
    {
        public SellerProductRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
