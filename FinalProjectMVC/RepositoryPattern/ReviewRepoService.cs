using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReviewRepoService : EFCoreRepo<Review>
    {
        public ReviewRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
