using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SellerProductRepoService : EFCoreRepo<SellerProduct>
    {
        public SellerProductRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
