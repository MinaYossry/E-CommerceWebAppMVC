using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.Constants;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.RepositoryPattern;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using FinalProjectMVC.ViewModels;
using Microsoft.CodeAnalysis;

namespace FinalProjectMVC.Controllers
{
    public class ReviewsController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Review> _reviewtRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            IRepository<Review> reviewtRepository ,
            IRepository<Product> productRepository,
            IRepository<Seller> sellerRepository,
            IRepository<Customer> customerRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _reviewtRepository = reviewtRepository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        // GET: Reviews
    
        public async Task<IActionResult> Index(int? productId , int? ratingFilter)
        {
            ViewBag.ProductList = new SelectList(new[] { _context.Products }, "ProductId", "ProductName");
            var reviews = await _reviewtRepository.FilterAsync(r => r.ProductId == productId);
            //var reviews = _productRepository.GetReviews(ProductId);
            // pass the product's ProductId to the GetReviews method
            if (ratingFilter.HasValue)
            {
                reviews = reviews.Where(r => r.Rating == ratingFilter.Value).ToList();
            }
            var applicationDbContext = _context.Reviews.Include(r => r.Customer).Include(r => r.Product).Include(r => r.Seller);
            return View(reviews);
        }
    
        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .Include(r => r.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_customerRepository.GetAll(), "Id", "Id");
            ViewData["ProductId"] = new SelectList(_productRepository.GetAll(), "Id", "Name");
            ViewData["SellerId"] = new SelectList(_sellerRepository.GetAll(), "Id", "Id");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddReviewViewModel model)
        {
            if (!User.IsInRole(Roles.Customer.ToString()))
            {
                throw new Exception("Not have Permisstion");
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
                     _reviewtRepository.Insert(review);
                }
                catch
                {
                    throw new Exception(" Please Try Again");
                }

                return RedirectToAction("Details","Products", new {area = "CustomerPanel", Id = model.ProductId, SellerId = model.SellerId});
            }
            return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = model.ProductId, SellerId = model.SellerId });

        }
        #region Create
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating,CustomerId,SellerId,ProductId")] Review review)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(review);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", review.CustomerId);
        //    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", review.ProductId);
        //    ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", review.SellerId);
        //    return View(review);
        //}

        #endregion
        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", review.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", review.ProductId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", review.SellerId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditReviewViewModel viewModel)

        {
            var review = _reviewtRepository.GetDetails(id);
           
           
            if (review == null)
            {
                return NotFound();
            }
            var userId = await _userManager.GetUserAsync(User);
           

            if ((review.CustomerId.ToLower().Trim() != userId.Id.ToString().ToLower().Trim()) && !User.IsInRole(Roles.Customer.ToString()))
            {
                throw new Exception("Not have Permisstion");
            }


            if (ModelState.IsValid)
        {
            review.Name = viewModel.Name;
            review.Description = viewModel.Description;
            review.Rating = viewModel.Rating;
            //review.CreatedDate = DateTime.Now;

            try
            {
                 _reviewtRepository.Update(id, review);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while updating the review.");
            }
        }

        return View();


        }
        #region Edit
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Rating,CustomerId,SellerId,ProductId")] Review review)
        //{
        //    if (id != review.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(review);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReviewExists(review.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", review.CustomerId);
        //    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", review.ProductId);
        //    ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", review.SellerId);
        //    return View(review);
        //}

        //public async Task<IActionResult> Edit(int id, EditReviewViewModel viewModel)
        //{
        //    if (!User.IsInRole(Roles.Admin.ToString()) || !User.IsInRole(Roles.Customer.ToString()))
        //    {
        //        throw new Exception("Not have Permisstion");
        //    }

        //    var review = _reviewtRepository.GetDetails(id);

        //    if (review == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        review.Name = viewModel.Name;
        //        review.Description = viewModel.Description;
        //        review.Rating = viewModel.Rating;
        //        //review.CreatedDate = DateTime.Now;

        //        try
        //        {
        //            _reviewtRepository.Update(id, review);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            ModelState.AddModelError("", "An error occurred while updating the review.");
        //        }
        //    }

        //    return View();
        //}
        #endregion

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            //if (User.Identity?.IsAuthenticated != true && !User.IsInRole(Roles.Admin.ToString()))
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            var review = _reviewtRepository.GetDetails(id);
            if (review == null)
                return NotFound();
            var userId = await _userManager.GetUserAsync(User);

            if ((review.CustomerId.ToLower().Trim() != userId.Id.ToString().ToLower().Trim()) && !User.IsInRole(Roles.Customer.ToString()))
            {
                throw new Exception("Not have Permisstion");
            }
            _reviewtRepository.Delete(id);
            return View();

        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Products", new { area = "CustomerPanel", Id = review.ProductId, SellerId = review.SellerId });
        }

        private bool ReviewExists(int id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
