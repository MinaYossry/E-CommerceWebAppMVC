using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationDbContext _context;
        readonly IRepository<Product> productRepo;
        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IRepository<Product> productRepo)
        {
            _logger = logger;
            _context = context;
            this.productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = (await productRepo.GetAllAsync()).Where(p => p.SellerProducts.Count > 0);

            var viewModel = new FrontPageViewModel()
            {
                BestSelllerProducts = products.Take(4).ToList(),
                FeaturedProducts = products.Skip(4).Take(4).ToList(),
            };

            return View(viewModel);
        }

        public IActionResult Privacy() => View();

        [Route("Home/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            string errorMessage;

            switch (statusCode)
            {
                case 400:
                    errorMessage = "Bad Request";
                    break;
                case 401:
                    errorMessage = "Unauthorized";
                    break;
                case 403:
                    errorMessage = "Forbidden";
                    break;
                case 404:
                    errorMessage = "Page Not Found";
                    break;
                case 500:
                    errorMessage = "Internal Server Error";
                    break;
                default:
                    var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    errorMessage = exception?.Message;
                    break;
            }

            ViewData["StatusCode"] = statusCode;
            ViewData["ErrorMessage"] = errorMessage;
            return View();
        }
    }
}