using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class OrderRepoService : EFCoreRepo<Order>
    {
        public OrderRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
