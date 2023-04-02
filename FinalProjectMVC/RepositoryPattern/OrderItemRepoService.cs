using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class OrderItemRepoService : EFCoreRepo<OrderItem>
    {
        public OrderItemRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
