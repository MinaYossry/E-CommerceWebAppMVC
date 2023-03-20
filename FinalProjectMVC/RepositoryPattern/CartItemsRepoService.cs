using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CartItemsRepoService : EFCoreRepo<CartItem>
    {
        public CartItemsRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
