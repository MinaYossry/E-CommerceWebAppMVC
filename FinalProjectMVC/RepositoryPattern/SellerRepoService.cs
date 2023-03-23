using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class SellerRepoService : EFCoreRepo<Seller>
    {
        public SellerRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
