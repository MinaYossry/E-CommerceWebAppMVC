using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class CustomerRepoService : EFCoreRepo<Customer>
    {
        public CustomerRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
