using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CartItemsRepoService : EFCoreRepo<CartItem>
    {
        public CartItemsRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
