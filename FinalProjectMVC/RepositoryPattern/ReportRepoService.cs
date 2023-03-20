using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReportRepoService : EFCoreRepo<Report>
    {
        public ReportRepoService(StoreDbContext context) : base(context)
        {
        }
    }
}
