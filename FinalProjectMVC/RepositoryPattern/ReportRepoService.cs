using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ReportRepoService : EFCoreRepo<Report>
    {
        public ReportRepoService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
