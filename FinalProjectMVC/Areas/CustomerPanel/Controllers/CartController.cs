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
        readonly ApplicationDbContext _context;

        readonly IRepository<Product> _productRepository;
        // private readonly IRepository<Seller> _sellerRepository;
        // private readonly IRepository<Category> _categoryRepository;
        // private readonly IRepository<Brand> _brandRepository;
        // private readonly IRepository<SubCategory> _subCategoryRepository;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        readonly IRepository<SellerProduct> _sellerProductRepo;
        readonly IRepository<CartItem> _cartItemRepo;
        readonly IRepository<Customer> _customerRepo;

        readonly UserManager<ApplicationUser> _userManager;

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

            IRepository<Customer> customerRepo,

            ApplicationDbContext context

            )
        {
            _productRepository = productRepository;
            // this._sellerRepository = sellerRepository;
            // this._categoryRepository = categoryRepository;
            // this._brandRepository = brandRepository;
            // this._subCategoryRepository = subCategoryRepository;
            // this._signInManager = signInManager;
            _sellerProductRepo = sellerProductRepo;
            _userManager = userManager;
            _cartItemRepo = cartItemRepo;
            _customerRepo = customerRepo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItemCount = (await _cartItemRepo.FilterAsync(sp => sp.CustomerId == userId));

            return View(cartItemCount);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCartItem([FromBody] CartItemUpdateRequest request)
        {
            // find the cart item with the given ID
            // var cartItem = _cartItemRepo. CartItems.SingleOrDefault(ci => ci.Id == request.CartItemId);

            var cartItem = (await _cartItemRepo.FilterAsync(sp => sp.Id == request.CartItemId)).SingleOrDefault();

            if (cartItem == null) return NotFound();
            var oldCount = cartItem.Count;
            var Price = cartItem.SellerProduct?.Price ?? 0;

            switch (request.Action)
            {
                case "increment":
                    cartItem.Count += 1;
                    break;
                case "decrement":
                    cartItem.Count -= 1;
                    break;
                case "remove":
                    await _cartItemRepo.DeleteAsync(request.CartItemId);
                    return Ok(new { oldCount, Price });
                default:
                    return BadRequest();
            }

            await _cartItemRepo.UpdateAsync(request.CartItemId, cartItem);

            return Ok(new { oldCount, cartItem.Count, Price });
        }

        public class CartItemUpdateRequest
        {
            public int CartItemId { get; set; }
            public string? Action { get; set; }
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

            if (user == null) return Unauthorized();

            var cartItem = new CartItem
            {
                CustomerId = user.Id,
                SellerProductId = sellerProductId,
                Count = count
            };

            await _cartItemRepo.InsertAsync(cartItem);

            // _context.CartItems.Add(cartItem);
            // await _context.SaveChangesAsync();

            // return RedirectToAction("Index");

            // Count property of List, You have to () to include the await.
            var cartItemCount = (await _cartItemRepo.FilterAsync(sp => sp.CustomerId == userId)).Count;
            return Ok(cartItemCount);

            // return View(model: userId);
        }

        public async Task<IActionResult> GetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Count property of List, You have to () to include the await.
            var cartItem = (await _cartItemRepo.FilterAsync(sp => sp.CustomerId == userId)).Count;
            return Ok(cartItem);
        }

        public IActionResult success(string id)
        {
            var cartItems = _context.CartItems.Where(item => item.CustomerId == id).ToList();
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
            return View();
        }

        public IActionResult cancel(int id)
        {
            var my_order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            _context.Orders.Remove(my_order);
            _context.SaveChanges();
            return View();
        }
    }
}