//using FinalProjectMVC.Areas.AdminPanel.Models;
//using FinalProjectMVC.Areas.Identity.Data;
//using FinalProjectMVC.Areas.SellerPanel.Models;
//using FinalProjectMVC.Areas.SellerPanel.ViewModel;
//using FinalProjectMVC.Constants;
//using FinalProjectMVC.Models;
//using FinalProjectMVC.RepositoryPattern;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Security.Claims;


using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.CustomerPanel.ViewModel;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.ViewModel;
using FinalProjectMVC.Constants;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Area("CustomerPanel")]
    //[Authorize(Roles = "Customer")] // I think authorization should be added on specific Actions
    public class ProductsController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        //private readonly IRepository<Seller> _sellerRepository;
        //private readonly IRepository<Category> _categoryRepository;
        //private readonly IRepository<Brand> _brandRepository;
        //private readonly IRepository<SubCategory> _subCategoryRepository;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<SellerProduct> _sellerProductRepo;

        public ProductsController(
            IRepository<Product> productRepository,
            ////IRepository<Seller> sellerRepository,
            //IRepository<Category> categoryRepository,
            //IRepository<Brand> brandRepository,
            //IRepository<SubCategory> subCategoryRepository,
            //SignInManager<ApplicationUser> signInManager,
            IRepository<SellerProduct> sellerProductRepo
            )

        {
            _productRepository = productRepository;
            //this._sellerRepository = sellerRepository;
            //this._categoryRepository = categoryRepository;
            //this._brandRepository = brandRepository;
            //this._subCategoryRepository = subCategoryRepository;
            //this._signInManager = signInManager;
            this._sellerProductRepo = sellerProductRepo;
        }

        // GET: SellerPanel/Products
        public async Task<IActionResult> Index(List<Product> filtered_products)
        {
            string json = TempData["data"] as string;
            //List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json)??new List<Product>();
            List<Product> products = new List<Product>();



            //List<Product> filtered = TempData["data"] as List<Product>;

            /* We have to use Any instead of where, as FilterAsync expects a `boolen`
             * While `Where` returns a List.
             * 
             * We can repeat this query but use X.Count == 0 , to return out
             */

            /* This LinQ (p => p.SellerProducts?.Any(x => x.Count > 0)
             * is important to get all the products that have someone that can sell them
              Which means ( not totally out of stock) 
            
                using navigational property (sellerProducts) helps be to access that table 
                to filter based on it.

            //var productList = await _productRepository.FilterAsync(p => p.SellerProducts?.Any(x => x.Count > 0) ?? false);

            //foreach (var product in productList)
            //{
            //    var sellerProducts = await _sellerProductRepo.FilterAsync(sp => sp.ProductId == product.Id && sp.Count > 0);
            //    if (sellerProducts != null)
            //    {
            //        var lowestPrice = sellerProducts.Min(sp => sp.Price);
            //        //product.LowestPrice = lowestPrice;
            //        product.SellerWithLowestPrice = sellerProducts.FirstOrDefault(sp => sp.Price == lowestPrice)?.Seller?.Name;
            //    }
            //}

            //return View(productList);
            */
            if (products.Count() != 0)
                products = products.Where(p => p.SellerProducts?.Any(x => x.Count > 0) ?? false).ToList();
            else
                products = await _productRepository.FilterAsync(p => p.SellerProducts?.Any(x => x.Count > 0) ?? false);

            var viewModelList = new List<DisplayInStockProductsViewModel>();

            foreach (var product in products)
            {
                 /*Now this linQ is also important to analyse the products we got, 
                  * Here we filter the SellerProduct table to get only the records of the 
                  * 1- sellers that sell that specific product, 
                  * 2- and they have that product currently In-stock
                  * 
                  * That's why we needed to ge filtered list out of Product table 
                    and a List out of `SellerProduct` Table. 
                 */
                 
                var sellerProducts = await _sellerProductRepo.FilterAsync(sp => sp.ProductId == product.Id && sp.Count > 0);

                if (sellerProducts != null)
                {
                    var lowestPrice = sellerProducts.Min(sp => sp.Price);

                    // Returns the  Cheapest instance of sold by a seller.
                    var sellerProductWithLowestPrice = sellerProducts
                        .FirstOrDefault(sp => sp.Price == lowestPrice);

                    var productViewModel = new DisplayInStockProductsViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductPrice= lowestPrice,
                        ProductDescription = product.Description,
                        ProductImage = product.ProductImage,

                        SellerId = sellerProductWithLowestPrice?.SellerId,
                        SellerNameWithLowestPrice = sellerProductWithLowestPrice?.Seller?.ApplicationUser?.FirstName,
                        Count = sellerProductWithLowestPrice.Count,

                        Brand =  product?.Brand?.Name,
                    SubCategory = product?.SubCategory?.Name

                    };

                    viewModelList.Add(productViewModel);

                }
            }

            //viewModel.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            //viewModel.SubCategories = new SelectList(await _subCategoryRepository.GetAllAsync(), "Id", "Name");

            return View(viewModelList);


        }

        
        // GET: SellerPanel/Products/Details/5
        [Route("Product/{id:int}/{SellerId}")]
        public async Task<IActionResult> Details(int? id, string? SellerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id.Value);

            // We recived the Current seller from Index and here we get the sellerProduct record for it.
            var sellerProductRow = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.SellerId == SellerId)).FirstOrDefault();
            if (sellerProductRow == null)
            {
                return NotFound();
            }

            var availableSellers = await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.Count > 0);

            ViewData["SellerName"] = new SelectList(availableSellers, "SellerId", "Seller.ApplicationUser.FirstName",SellerId);


            var DetailedProductviewModel = new DetailedProductViewModel
            {
                ProductId = sellerProductRow.ProductId,
                ProductName = sellerProductRow.Product?.Name,
                ProductPrice = sellerProductRow.Price,
                ProductDescription = sellerProductRow.Product?.Description,
                ProductImage = sellerProductRow.Product?.ProductImage,

                SellerName = sellerProductRow.Seller?.ApplicationUser?.FirstName,
                SellerId = sellerProductRow.SellerId,
                Count = sellerProductRow.Count,

                CurrentSellerProduct = sellerProductRow,

                Brand = sellerProductRow.Product?.Brand?.Name,
                SubCategory = sellerProductRow.Product?.SubCategory?.Name,


                //SellersList = availableSellers
                //SellersList = new SelectList(availableSellers, "SellerId", "Seller?.ApplicationUser?.FirstName")


            };

            //ViewBag.SellingList = new SelectList(availableSellers, "SellerId", "Seller?.ApplicationUser?.FirstName");

            return View(DetailedProductviewModel);
        }









        //public async Task<IActionResult> Create()
        //{
        //    if (User.Identity?.IsAuthenticated != true || !User.IsInRole(Roles.Seller.ToString()))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var userId = User.GetUserId();
        //    if (userId is null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var viewModel = new AddProductViewModel
        //    {
        //        Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name"),
        //        SubCategories = new SelectList(await _subCategoryRepository.GetAllAsync(), "Id", "Name"),
        //        SellerID = userId
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(AddProductViewModel viewModel)
        //{
        //    var productExist = (await _productRepository.FilterAsync(p => p.SerialNumber == viewModel.SerialNumber)).FirstOrDefault();

        //    if (productExist is not null)
        //    {
        //        var sellerHaveProduct = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == productExist.Id && sp.SellerId == viewModel.SellerID)).FirstOrDefault();
        //        if (sellerHaveProduct is not null)
        //        {
        //            ModelState.AddModelError("Already Exist", "Sorry you already have this product for sale");
        //        }
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var product = new Product
        //        {
        //            SerialNumber = viewModel.SerialNumber,
        //            Name = viewModel.ProductName,
        //            Description = viewModel.ProductDescription,
        //            SubCategoryId = viewModel.SubCategoryId,
        //            BrandId = viewModel.BrandId
        //        };

        //        if (viewModel.formFile is not null && viewModel.formFile.Length > 0)
        //        {
        //            using var ms = new MemoryStream();
        //            await viewModel.formFile.CopyToAsync(ms);
        //            product.ProductImage = ms.ToArray();
        //        }

        //        if (productExist is null)
        //        {
        //            try
        //            {
        //               await _productRepository.InsertAsync(product);
        //            }
        //            catch
        //            {
        //                throw new Exception("Can't Add new Product");
        //            }

        //            productExist = product;
        //        }

        //        var newItem = new SellerProduct
        //        {
        //            SellerId = viewModel.SellerID,
        //            ProductId = productExist.Id,
        //            Count = viewModel.Count,
        //            Price = viewModel.Price
        //        };

        //        try
        //        {
        //           await _sellerProductRepo.InsertAsync(newItem);
        //        }
        //        catch 
        //        {
        //            throw new Exception("You Already have this product in sale");
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    viewModel.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
        //    viewModel.SubCategories = new SelectList(await _subCategoryRepository.GetAllAsync(), "Id", "Name");
        //    return View(viewModel);
        //}


        //// GET: SellerPanel/Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (!User.IsSeller())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
        //    if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    if (sellerProduct.Product == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new EditSellerProductViewModel()
        //    {
        //        SellerProductId = sellerProduct.Id,
        //        SerialNumber = sellerProduct.Product.SerialNumber,
        //        Name = sellerProduct.Product.Name,
        //        Description = sellerProduct.Product.Description,
        //        ProductImage = sellerProduct.Product.ProductImage,
        //        Count = sellerProduct.Count,
        //        Price = sellerProduct.Price
        //    };

        //    return View(viewModel);
        //}

        //// POST: SellerPanel/Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EditSellerProductViewModel viewModel)
        //{
        //    if (!User.IsSeller())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
        //    if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        sellerProduct.Count = viewModel.Count;
        //        sellerProduct.Price = viewModel.Price;

        //        try
        //        {
        //            await _sellerProductRepo.UpdateAsync(id, sellerProduct);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            ModelState.AddModelError("", "An error occurred while updating the product.");
        //        }
        //    }

        //    if (sellerProduct?.Product is not null)
        //    {
        //        viewModel.SerialNumber = sellerProduct.Product.SerialNumber;
        //        viewModel.Name = sellerProduct.Product.Name;
        //        viewModel.Description = sellerProduct.Product?.Description;
        //        viewModel.ProductImage = sellerProduct.Product?.ProductImage;
        //    }

        //    return View(viewModel);
        //}

        //// GET: SellerPanel/Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (!User.IsSeller())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var sellerProduct = (await _sellerProductRepo.FilterAsync(sp => sp.ProductId == id && sp.SellerId == User.GetUserId())).FirstOrDefault();
        //    if (sellerProduct == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    if (sellerProduct.Product == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new DeleteSellerProductViewModel()
        //    {
        //        SellerProductId = sellerProduct.Id,
        //        SerialNumber = sellerProduct.Product.SerialNumber,
        //        Name = sellerProduct.Product.Name,
        //        Description = sellerProduct.Product.Description,
        //        ProductImage = sellerProduct.Product.ProductImage,
        //        Count = sellerProduct.Count,
        //        Price = sellerProduct.Price
        //    };

        //    return View(viewModel);
        //}

        ////    // POST: SellerPanel/Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (!User.IsSeller())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var sellerProduct = await _sellerProductRepo.GetDetailsAsync(id);
        //    if (sellerProduct == null || !(sellerProduct.SellerId == User.GetUserId()))
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    try
        //    {
        //        await _sellerProductRepo.DeleteAsync(sellerProduct.Id);
        //    }

        //    catch
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}

        //private bool ProductExists(int id)
        //{
        //    return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

