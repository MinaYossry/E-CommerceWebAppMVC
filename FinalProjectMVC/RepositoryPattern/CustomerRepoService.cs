using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CustomerRepoService : EFCoreRepo<Customer>
    {
        public CustomerRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
