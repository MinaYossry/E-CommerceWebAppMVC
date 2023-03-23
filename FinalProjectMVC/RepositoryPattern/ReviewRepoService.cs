using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReviewRepoService : EFCoreRepo<Review>
    {
        public ReviewRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
