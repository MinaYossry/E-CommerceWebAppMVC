using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Area("CustomerPanel")]
    [Authorize(Roles = "Customer")] // I think authorization should be added on specific Actions

    public class CartController : Controller
    {

        private readonly IRepository<Product> _productRepository;
        //private readonly IRepository<Seller> _sellerRepository;
        //private readonly IRepository<Category> _categoryRepository;
        //private readonly IRepository<Brand> _brandRepository;
        //private readonly IRepository<SubCategory> _subCategoryRepository;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<SellerProduct> _sellerProductRepo;
        private readonly IRepository<CartItem> _cartItemRepo;
        private readonly IRepository<Customer> _customerRepo;


        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            IRepository<Product> productRepository,
            ////IRepository<Seller> sellerRepository,
            //IRepository<Category> categoryRepository,
            //IRepository<Brand> brandRepository,
            //IRepository<SubCategory> subCategoryRepository,
            //SignInManager<ApplicationUser> signInManager,
            IRepository<SellerProduct> sellerProductRepo,
            UserManager<ApplicationUser> userManager,

            IRepository<CartItem> cartItemRepo,
               
            IRepository<Customer> customerRepo

            )

        {
            _productRepository = productRepository;
            //this._sellerRepository = sellerRepository;
            //this._categoryRepository = categoryRepository;
            //this._brandRepository = brandRepository;
            //this._subCategoryRepository = subCategoryRepository;
            //this._signInManager = signInManager;
            this._sellerProductRepo = sellerProductRepo;
            _userManager = userManager;
            _cartItemRepo = cartItemRepo;
            _customerRepo = customerRepo;
        }





        [HttpPost]
        public async Task<IActionResult> AddToCart(int sellerProductId, int count)
        {
                /* User is a built-in property.*/

            // This line returns the user object. 
            var user = await _userManager.GetUserAsync(User);

            // This line returns the userId only. 
            // UserId is the same as customerId due to 1:1 relation
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           

            if (user == null)
            {
                return Unauthorized();
            }

            var cartItem = new CartItem
            {
                CustomerId = user.Id,
                SellerProductId = sellerProductId,
                Count = count
            };

            await _cartItemRepo.InsertAsync( cartItem );

            //_context.CartItems.Add(cartItem);
            //await _context.SaveChangesAsync();

            //return RedirectToAction("Index");

            // Count property of List, You have to () to include the await.
            int cartItemCount = (await _cartItemRepo.FilterAsync(sp => sp.CustomerId == userId)).Count;
            return Ok(cartItemCount);

            //return View(model: userId);
        }


        public async Task<IActionResult> GetCartCount()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Count property of List, You have to () to include the await.
            int cartItem = (await _cartItemRepo.FilterAsync(sp => sp.CustomerId == userId)).Count;
            return Ok(cartItem);
        }
    }
}






