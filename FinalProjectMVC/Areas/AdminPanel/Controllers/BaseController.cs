using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    public class BaseController : Controller
    {

        //private readonly ApplicationDbContext Context;

        public BaseController(IRepository<Report> _reportRepo)
        {
            //Context = context;
        }
        //When Any Action In Admin Controller Exectuted Load Some view bags to the layout
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            //var OrdersCount = Context.Orders.Count();
            //ViewBag.NewOrders = OrdersCount;

            //var ProductsCount = Context.Products.Count();
            //ViewBag.NewProducts = ProductsCount;

            //var SellersCount = Context.Sellers.Count();
            //ViewBag.Sellers = SellersCount;

            //var CustomersCount = Context.Customers.Count();
            //ViewBag.Customers = CustomersCount;

            //var Reports = Context.Reports.Count();
            //ViewBag.ReportsCount = Reports;

            base.OnActionExecuting(filterContext);
        }

    }
}
