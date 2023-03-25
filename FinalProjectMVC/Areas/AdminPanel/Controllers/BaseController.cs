using FinalProjectMVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    public class BaseController : Controller
    {

        private readonly ApplicationDbContext Context;

        public BaseController(ApplicationDbContext context)
        {
            Context = context;
        }
        //When Any Action In Admin Controller Exectuted Load Some view bags to the layout
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            var OrdersCount = Context.Orders.Count();
            ViewBag.NewOrders = OrdersCount;

            var ProductsCount = Context.Products.Count();
            ViewBag.NewProducts = ProductsCount;

            var SellersCount = Context.Sellers.Count();
            ViewBag.Sellers = SellersCount;

            var CustomersCount = Context.Customers.Count();
            ViewBag.Customers = CustomersCount;

            var Reports = Context.Reports.Count();
            ViewBag.ReportsCount = Reports;

            if (Reports > 0)
            {
                for (int i = 0; i < Reports && i < 3; i++)
                {
                        var report = Context.Reports.Skip(i).First();

            
                        var reportName = report.Name;
                        var createdDate = report.CreatedDate;

                        switch (i)
                        {
                            case 0:
                                ViewBag.Report1ProductName = reportName;
                                ViewBag.Report1CreatedDate = createdDate;
                                break;
                            case 1:
                                ViewBag.Report2ProductName = reportName;
                                ViewBag.Report2CreatedDate = createdDate;
                                break;
                            case 2:
                                ViewBag.Report3ProductName = reportName;
                                ViewBag.Report3CreatedDate = createdDate;
                                break;
                        }
                    
 
                }
            }


            base.OnActionExecuting(filterContext);
        }

    }
}
