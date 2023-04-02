using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReportRepoService : EFCoreRepo<Report>
    {
        public ReportRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
