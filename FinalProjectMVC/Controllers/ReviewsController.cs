using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Constants;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        readonly IRepository<Review> _reviewtRepository;

        public ReviewsController(IRepository<Review> reviewtRepository)
        {
            _reviewtRepository = reviewtRepository;
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddReviewViewModel model)
        {
            if (!User.IsInRole(Roles.Customer.ToString()) || User.GetUserId() != model.CustomerId)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var review = new Review
                {
                    Name = model.Name,
                    Description = model.Description,
                    CustomerId = model.CustomerId,
                    SellerId = model.SellerId,
                    ProductId = model.ProductId
                };

                try
                {
                    await _reviewtRepository.InsertAsync(review);
                }
                catch
                {
                    return BadRequest();
                }

                return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = model.ProductId, SellerId = model.SellerId });
            }

            return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = model.ProductId, SellerId = model.SellerId });
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _reviewtRepository.GetDetailsAsync(id);

            if (review is null) return BadRequest();

            if (User.GetUserId() != review.CustomerId) return Unauthorized();

            if (review != null)
            {
                await _reviewtRepository.DeleteAsync(id);
            }

            if (review is not null)
                return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = review.ProductId, SellerId = review.SellerId });
            else
                return RedirectToAction("Index", "Products");
        }
    }
}