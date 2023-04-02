using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SellerRepoService : EFCoreRepo<Seller>
    {
        public SellerRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
