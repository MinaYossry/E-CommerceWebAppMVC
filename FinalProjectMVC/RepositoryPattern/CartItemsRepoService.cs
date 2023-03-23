using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CartItemsRepoService : EFCoreRepo<CartItem>
    {
        public CartItemsRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
