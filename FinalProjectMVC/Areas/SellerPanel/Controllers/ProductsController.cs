using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.ViewModel;
using FinalProjectMVC.Constants;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


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
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(Roles.Seller.ToString()))
            {
                var UserID = User.GetUserId();
                var ProductList = await _sellerProductRepo.FilterAsync(p => p.SellerId == UserID);

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

            var userId = User.GetUserId();
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentProduct = await _productRepository.GetDetailsAsync(id);
            if (currentProduct is null)
            {
                return NotFound();
            }

            var sellerProduct = (await _sellerProductRepo.FilterAsync(sp => sp.SellerId == userId && sp.ProductId == currentProduct.Id)).FirstOrDefault();
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

        public async Task<IActionResult> Create()
        {
            if (User.Identity?.IsAuthenticated != true || !User.IsInRole(Roles.Seller.ToString()))
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.GetUserId();
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AddProductViewModel
            {
                Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name"),
                SubCategories = new SelectList(await _subCategoryRepository.GetAllAsync(), "Id", "Name"),
                SellerID = userId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductViewModel viewModel)
        {
            var productExist = (await _productRepository.FilterAsync(p => p.SerialNumber == viewModel.SerialNumber)).FirstOrDefault();

            if (productExist is not null)
            {
                var sellerHaveProduct = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == productExist.Id && sp.SellerId == viewModel.SellerID)).FirstOrDefault();
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
                       await _productRepository.InsertAsync(product);
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
                   await _sellerProductRepo.InsertAsync(newItem);
                }
                catch 
                {
                    throw new Exception("You Already have this product in sale");
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            viewModel.SubCategories = new SelectList(await _subCategoryRepository.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }


        // GET: SellerPanel/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsSeller())
            {
                return RedirectToAction("Index");
            }

            var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
            if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
            {
                return RedirectToAction("Index");
            }

            if (sellerProduct.Product == null)
            {
                return NotFound();
            }

            var viewModel = new EditSellerProductViewModel()
            {
                SellerProductId = sellerProduct.Id,
                SerialNumber = sellerProduct.Product.SerialNumber,
                Name = sellerProduct.Product.Name,
                Description = sellerProduct.Product.Description,
                ProductImage = sellerProduct.Product.ProductImage,
                Count = sellerProduct.Count,
                Price = sellerProduct.Price
            };

            return View(viewModel);
        }

        // POST: SellerPanel/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSellerProductViewModel viewModel)
        {
            if (!User.IsSeller())
            {
                return RedirectToAction("Index");
            }

            var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
            if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                sellerProduct.Count = viewModel.Count;
                sellerProduct.Price = viewModel.Price;

                try
                {
                    await _sellerProductRepo.UpdateAsync(id, sellerProduct);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the product.");
                }
            }

            if (sellerProduct?.Product is not null)
            {
                viewModel.SerialNumber = sellerProduct.Product.SerialNumber;
                viewModel.Name = sellerProduct.Product.Name;
                viewModel.Description = sellerProduct.Product?.Description;
                viewModel.ProductImage = sellerProduct.Product?.ProductImage;
            }

            return View(viewModel);
        }

        // GET: SellerPanel/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsSeller())
            {
                return RedirectToAction("Index");
            }

            var sellerProduct = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.SellerId == User.GetUserId())).FirstOrDefault();
            if (sellerProduct == null)
            {
                return RedirectToAction("Index");
            }

            if (sellerProduct.Product == null)
            {
                return NotFound();
            }

            var viewModel = new DeleteSellerProductViewModel()
            {
                SellerProductId = sellerProduct.Id,
                SerialNumber = sellerProduct.Product.SerialNumber,
                Name = sellerProduct.Product.Name,
                Description = sellerProduct.Product.Description,
                ProductImage = sellerProduct.Product.ProductImage,
                Count = sellerProduct.Count,
                Price = sellerProduct.Price
            };

            return View(viewModel);
        }

        //    // POST: SellerPanel/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsSeller())
            {
                return RedirectToAction("Index");
            }

            var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
            if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
            {
                return RedirectToAction("Index");
            }

            try
            {
                await _sellerProductRepo.DeleteAsync(sellerProduct.Id);
            }

            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //private bool ProductExists(int id)
        //{
        //    return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

