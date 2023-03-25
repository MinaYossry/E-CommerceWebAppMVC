using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.ViewModel;
using FinalProjectMVC.Constants;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FinalProjectMVC.Areas.SellerPanel.Controllers
{
    [Area("SellerPanel")]
    [Authorize(Roles = "Seller")]
    public class ProductsController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<SubCategory> _subCategoryRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<SellerProduct> _sellerProductRepo;

        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<Seller> sellerRepository,
            IRepository<Category> categoryRepository,
            IRepository<Brand> brandRepository,
            IRepository<SubCategory> subCategoryRepository,
            SignInManager<ApplicationUser> signInManager,
            IRepository<SellerProduct> sellerProductRepo
            )

        {
            _productRepository = productRepository;
            this._sellerRepository = sellerRepository;
            this._categoryRepository = categoryRepository;
            this._brandRepository = brandRepository;
            this._subCategoryRepository = subCategoryRepository;
            this._signInManager = signInManager;
            this._sellerProductRepo = sellerProductRepo;
        }

        // GET: SellerPanel/Products
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(Roles.Seller.ToString()))
            {
                var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var ProductList = _sellerProductRepo.Filter(p => p.SellerId == UserID);

                return View(ProductList);
            }
            else
            {
                return View(null);
            }
        }

        // GET: SellerPanel/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity?.IsAuthenticated != true || !User.IsInRole(Roles.Seller.ToString()))
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentProduct = _productRepository.GetDetails(id);
            if (currentProduct is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var sellerProduct = _sellerProductRepo.Filter(sp => sp.SellerId == userId && sp.ProductId == currentProduct.Id).FirstOrDefault();
            if (sellerProduct is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new DisplaySellerProductDetailesViewModel()
            {
                SellerProductId = sellerProduct.Id,
                SerialNumber = currentProduct.SerialNumber,
                Name = currentProduct.Name,
                Description = currentProduct.Description,
                ProductImage = currentProduct.ProductImage,
                Count = sellerProduct.Count,
                Price = sellerProduct.Price
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            if (User.Identity?.IsAuthenticated != true || !User.IsInRole(Roles.Seller.ToString()))
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AddProductViewModel
            {
                Brands = new SelectList(_brandRepository.GetAll(), "Id", "Name"),
                SubCategories = new SelectList(_subCategoryRepository.GetAll(), "Id", "Name"),
                SellerID = userId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductViewModel viewModel)
        {
            var productExist = _productRepository.Filter(p => p.SerialNumber == viewModel.SerialNumber).FirstOrDefault();

            if (productExist is not null)
            {
                var sellerHaveProduct = _sellerProductRepo.Filter(sp => sp.ProductId == productExist.Id && sp.SellerId == viewModel.SellerID).FirstOrDefault();
                if (sellerHaveProduct is not null)
                {
                    ModelState.AddModelError("Already Exist", "Sorry you already have this product for sale");
                }
            }

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    SerialNumber = viewModel.SerialNumber,
                    Name = viewModel.ProductName,
                    Description = viewModel.ProductDescription,
                    SubCategoryId = viewModel.SubCategoryId,
                    BrandId = viewModel.BrandId
                };

                if (viewModel.formFile is not null && viewModel.formFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await viewModel.formFile.CopyToAsync(ms);
                    product.ProductImage = ms.ToArray();
                }

                if (productExist is null)
                {
                    try
                    {
                        _productRepository.Insert(product);
                    }
                    catch
                    {
                        throw new Exception("Can't Add new Product");
                    }

                    productExist = product;
                }

                var newItem = new SellerProduct
                {
                    SellerId = viewModel.SellerID,
                    ProductId = productExist.Id,
                    Count = viewModel.Count,
                    Price = viewModel.Price
                };

                try
                {
                    _sellerProductRepo.Insert(newItem);
                }
                catch (Exception ex)
                {
                    throw new Exception("You Already have this product in sale");
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.Brands = new SelectList(_brandRepository.GetAll(), "Id", "Name");
            viewModel.SubCategories = new SelectList(_subCategoryRepository.GetAll(), "Id", "Name");
            return View(viewModel);
        }

        //


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