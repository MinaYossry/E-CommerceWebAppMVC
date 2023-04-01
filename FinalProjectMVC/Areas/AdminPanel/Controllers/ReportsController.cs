using Microsoft.AspNetCore.Mvc;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.Areas.AdminPanel.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ReportsController : Controller
    {
       // private readonly ApplicationDbContext _context;
        private readonly IRepository<Report> reportRepo;
        private readonly IRepository<Review> reviewRepo;

        public ReportsController(
            //ApplicationDbContext context,
            IRepository<Report> reportRepo,
            IRepository<Review> reviewRepo
            )
        {
        //    _context = context;
            this.reportRepo = reportRepo;
            this.reviewRepo = reviewRepo;
        }

        // GET: AdminPanel/Reports
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            var viewMode = new List<AdminReportsReviewsViewModel>();

            var ReportsList = (await reportRepo.GetAllAsync()).OrderBy(x => x.IsSolved).ToList();

            foreach (var Report in ReportsList)
            {
                viewMode.Add(new()
                {
                    ReportId = Report.Id,
                    IsReviewDeleted = Report.Review?.IsDeleted != false,
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
                    ApplicationUserId = Report?.ApplicationUserId ?? "",
                    ApplicationUserName = $"{Report?.ApplicationUser?.FirstName} {Report?.ApplicationUser?.LastName}",
                }
                ); ;
            }


            return View(viewMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int Id)
        {
            // Find the report with the given ID
            var report = await reportRepo.GetDetailsAsync(Id);
            if (report is null)
            {
                return NotFound();
            }

            // If the report is already marked as solved, return an error
            if (report.IsSolved)
            {
                ModelState.AddModelError("", "This report has already been marked as solved.");
            }

            if (report.Review?.IsDeleted == true)
            {
                report.IsSolved = true;
                report.SolveDate = DateTime.Now;

                try
                {
                    await reportRepo.UpdateAsync(Id, report);
                }
                catch
                {
                    throw new Exception("Couldn't update report");
                }
            }

            else
            {
                report.IsSolved = true;
                report.SolveDate = DateTime.Now;

                report.Review.IsDeleted = true;
                try
                {
                    await reportRepo.UpdateAsync(Id, report);
                    await reviewRepo.UpdateAsync(report.ReviewId, report.Review);
                }
                catch
                {
                    throw new Exception("Couldn't update report or review");
                }
            }

            // Redirect to the report list page
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsSolved(int Id)
        {
            // Find the report with the given ID
            var report = await reportRepo.GetDetailsAsync(Id);
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
                    await reportRepo.UpdateAsync(Id, report);
                }
                catch
                {
                    throw new Exception("Couldn't update report");
                }
            }

            // Redirect to the report list page
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/CreateReport")]
        public async Task<IActionResult> Create(Report report, int ProductId, string SellerId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await reportRepo.InsertAsync(report);
                }
                catch
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = ProductId, SellerId });
            }

            return BadRequest();
        }


    }
}
