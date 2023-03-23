using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class OrderRepoService : EFCoreRepo<Order>
    {
        public OrderRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
