using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.Areas.AdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using FinalProjectMVC.Constants;
using System.Security.Claims;

namespace FinalProjectMVC.Areas.SellerPanel.Controllers
{
    [Area("SellerPanel")]
    [Authorize(Roles = "Admin,Seller")]
    public class ProductsController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<SellerProduct> _sellerProductRepo;

        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<Seller> sellerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Brand> brandRepository,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IRepository<SellerProduct> sellerProductRepo
            )

        {
            _productRepository = productRepository;
            this._sellerRepository = sellerRepository;
            this._categoryRepository = categoryRepository;
            this._brandRepository = brandRepository;
            this._signInManager = signInManager;
            this._context = context;
            this._sellerProductRepo = sellerProductRepo;
        }

        // GET: SellerPanel/Products
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(Roles.Seller.ToString()))
            {
                var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentSeller = _sellerRepository.GetDetails(UserID);

                //ViewBag.Categories = _context.Categories.Include(c => c.SubCategories).ToList();
                //ViewBag.Brands = _context.Brands.ToList();
                var ProductList = _context.SellerProducts.Where(s => s.SellerId == UserID).Include(s => s.Product).ToList();

                return View(ProductList);
            }
            else
            {
                return View(null);
            }
        }

        // GET: SellerPanel/Products/Details/5
        //    public async Task<IActionResult> Details(int? id)
        //    {
        //        if (id == null || _context.Products == null)
        //        {
        //            return NotFound();
        //        }

        //        var product = await _context.Products
        //            .Include(p => p.Brand)
        //            .Include(p => p.SubCategory)
        //            .FirstOrDefaultAsync(m => m.Id == id);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(product);
        //    }

        // GET: SellerPanel/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_brandRepository.GetAll(), "Id", "Name");
            ViewData["SubCategoryId"] = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: SellerPanel/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerialNumber,Name,Description,SubCategoryId,BrandId")] Product product, IFormFile file, string SellerID, int Price, int Count)
        {
            if (ModelState.IsValid)
            {
                if (file is not null && file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        product.ProductImage = ms.ToArray();
                    }
                }

                // If other seller added the same product, don't create new one
                var productExist = await _context.Products.FirstOrDefaultAsync(p => p.SerialNumber == product.SerialNumber);
                if (productExist is null)
                {
                    _productRepository.Insert(product);
                    productExist = product;
                }

                SellerProduct newItem = new()
                { 
                    SellerId = SellerID, ProductId = productExist.Id, Count = Count, Price = Price
                };
                try
                {
                    _sellerProductRepo.Insert(newItem);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("You Already have this product in sale");
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
            return View(product);
        }

        //    // GET: SellerPanel/Products/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null || _context.Products == null)
        //        {
        //            return NotFound();
        //        }

        //        var product = await _context.Products.FindAsync(id);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        //        ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
        //        return View(product);
        //    }

        //    // POST: SellerPanel/Products/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("Id,SerialNumber,Name,Description,ProductImage,SubCategoryId,BrandId")] Product product)
        //    {
        //        if (id != product.Id)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(product);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!ProductExists(product.Id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        //        ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
        //        return View(product);
        //    }

        //    // GET: SellerPanel/Products/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null || _context.Products == null)
        //        {
        //            return NotFound();
        //        }

        //        var product = await _context.Products
        //            .Include(p => p.Brand)
        //            .Include(p => p.SubCategory)
        //            .FirstOrDefaultAsync(m => m.Id == id);
        //        if (product == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(product);
        //    }

        //    // POST: SellerPanel/Products/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        if (_context.Products == null)
        //        {
        //            return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        //        }
        //        var product = await _context.Products.FindAsync(id);
        //        if (product != null)
        //        {
        //            _context.Products.Remove(product);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool ProductExists(int id)
        //    {
        //      return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        //    }
        //}
    }

}