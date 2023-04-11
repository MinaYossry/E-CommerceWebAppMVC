using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CustomerRepoService : EFCoreRepo<Customer>
    {
        public CustomerRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
