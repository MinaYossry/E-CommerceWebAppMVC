using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class OrderRepoService : EFCoreRepo<Order>
    {
        public OrderRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
