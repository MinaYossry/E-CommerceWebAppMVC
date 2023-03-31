using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class AddressRepService : EFCoreRepo<Address>
    {
        public AddressRepService(ApplicationDbContext context) : base(context) { }
    }
}
