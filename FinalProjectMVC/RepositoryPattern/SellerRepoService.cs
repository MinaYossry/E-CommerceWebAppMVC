using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SellerRepoService : EFCoreRepo<Seller>
    {
        public SellerRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
