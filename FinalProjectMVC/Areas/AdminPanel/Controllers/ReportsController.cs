using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.Areas.AdminPanel.ViewModel;
using Castle.Core.Resource;
using FinalProjectMVC.Areas.SellerPanel.Models;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ReportsController : Controller
    {
       // private readonly ApplicationDbContext _context;
        private readonly IRepository<Report> reportRepo;

        public ReportsController(
            //ApplicationDbContext context,
            IRepository<Report> reportRepo
            )
        {
        //    _context = context;
            this.reportRepo = reportRepo;
        }

        // GET: AdminPanel/Reports
        public async Task<IActionResult> Index()
        {

            var viewMode = new List<AdminReportsReviewsViewModel>();

            var ReportsList = (await reportRepo.GetAllAsync()).OrderBy(x => x.IsSolved).ToList();

            foreach (var Report in ReportsList)
            {
                viewMode.Add(new()
                    {
                        ReportId = Report.Id,
                        Name = Report.Name,
                        Description = Report.Description,
                        IsSolved = Report.IsSolved,
                        SolveDate = Report.SolveDate,
                        CreatedDate = Report.CreatedDate,
                        ReviewId = Report.ReviewId,
                        ReviewName = Report.Review?.Name ?? "",
                        ReviewDescription = Report.Review?.Description ?? "",
                        SellerId = Report.Review?.SellerId ?? "",
                        SellerName = $"{Report.Review?.Seller?.ApplicationUser?.FirstName} {Report.Review?.Seller?.ApplicationUser?.LastName}",
                        CustomerId = Report.Review?.CustomerId ?? "",
                        CustomerName = $"{Report.Review?.Customer?.ApplicationUser?.FirstName} {Report.Review?.Customer?.ApplicationUser?.LastName}",
                }
                );
            }


            return View(viewMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsSolved(int id)
        {
            // Find the report with the given ID
            var report = await reportRepo.GetDetailsAsync(id);
            if (report is null)
            {
                return NotFound();
            }

            // If the report is already marked as solved, return an error
            if (report.IsSolved)
            {
                ModelState.AddModelError("", "This report has already been marked as solved.");
            }

            // Update the "IsSolved" property of the report and save the changes to the database
            else
            {
                report.IsSolved = true;
                report.SolveDate = DateTime.Now;
                try
                {
                    await reportRepo.UpdateAsync(id, report);
                }
                catch
                {
                    throw new Exception("Couldn't update report");
                }
            }

            // Redirect to the report list page
            return RedirectToAction("Index");
        }

        //// GET: AdminPanel/Reports/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Reports == null)
        //    {
        //        return NotFound();
        //    }

        //    var report = await _context.Reports
        //        .Include(r => r.Review)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (report == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(report);
        //}

        //// GET: AdminPanel/Reports/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Reports == null)
        //    {
        //        return NotFound();
        //    }

        //    var report = await _context.Reports
        //        .Include(r => r.Review)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (report == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(report);
        //}

        //// POST: AdminPanel/Reports/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Reports == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Reports'  is null.");
        //    }
        //    var report = await _context.Reports.FindAsync(id);
        //    if (report != null)
        //    {
        //        _context.Reports.Remove(report);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        //private bool ReportExists(int id)
        //{
        //  return (_context.Reports?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
