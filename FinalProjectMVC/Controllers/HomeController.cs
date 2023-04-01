using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FinalProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.Include(c => c.SubCategories).ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult showdata(int id)
        {
            var products = _context.Products.Where(p=>p.SubCategoryId== id).ToList();
            var routeValues = new RouteValueDictionary();
            routeValues.Add("area", "CustomerPanel");
            routeValues.Add("filtered_Products", products);
            //var json = JsonConvert.SerializeObject(products);
            string json = JsonConvert.SerializeObject(products, Formatting.None,
    new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });
            TempData["data"] = json;

            return RedirectToAction("Index", "Products", routeValues);
            //return RedirectToAction("Index", "Products", new { area = "CustomerPanel", filtered_Products = products });

            //return View(products);
        }

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