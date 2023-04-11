using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReviewRepoService : EFCoreRepo<Review>
    {
        public ReviewRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}