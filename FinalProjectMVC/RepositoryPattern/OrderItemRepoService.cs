using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class OrderItemRepoService : EFCoreRepo<OrderItem>
    {
        public OrderItemRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
