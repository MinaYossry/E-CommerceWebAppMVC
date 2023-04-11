using FinalProjectMVC.Areas.CustomerPanel.ViewModel;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Area("CustomerPanel")]
    [Authorize(Roles = "Customer")]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        readonly ApplicationDbContext _context;
        readonly IRepository<Order> orderRepo;
        readonly IRepository<OrderItem> orderItemRepo;

        public OrdersController(ApplicationDbContext context, IRepository<Order> orderRepo, IRepository<OrderItem> orderItemRepo)
        {
            _context = context;
            this.orderRepo = orderRepo;
            this.orderItemRepo = orderItemRepo;
        }

        // GET: CustomerPanel/Orders
        public async Task<IActionResult> Index()
        {
            var customerOrders = (await orderRepo.FilterAsync(o => o.CustomerId == User.GetUserId()));

            var viewModel = new List<CustomerOrdersViewModel>();

            foreach (var order in customerOrders)
            {
                var orderViewModel = MapOrderModel(order);

                viewModel.Add(orderViewModel);
            }

            return View(viewModel);
        }

        // GET: CustomerPanel/Orders/Details/5
        [Route("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await orderRepo.GetDetailsAsync(id);

            if (order == null) return NotFound();

            var orderViewModel = MapOrderModel(order);
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cancel")]
        public async Task<IActionResult> CancelOrder(int Id)
        {
            var orderItem = await orderItemRepo.GetDetailsAsync(Id);

            if (orderItem is null) return NotFound();

            if (orderItem.Status > OrderStatus.Pending)
                ModelState.AddModelError("", "Sorry Can't Cancel Order now");

            try
            {
                orderItem.Status = OrderStatus.Cancelled;
                await orderItemRepo.UpdateAsync(Id, orderItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Details), new { id = orderItem.OrderId });
        }


        bool OrderExists(int id) => (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();

        CustomerOrdersViewModel? MapOrderModel(Order order)
        {
            if (order is not null)
            {
                var orderViewModel = new CustomerOrdersViewModel()
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    TotalPrice = order.TotalPrice,
                    OrderDate = order.OrderDate,
                    OrderItems = new(),
                    Address = order.Address
                };

                if (order.OrderItems is not null)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        orderViewModel.OrderItems
                            .Add(new()
                        {
                            Id = orderItem.Id,
                            Status = orderItem.Status,
                            SellerProductId = orderItem.SellerProductId,
                            Count = orderItem.Count,
                            Price = orderItem.Price,
                            Seller = orderItem.SellerProduct.Seller,
                            Product = orderItem.SellerProduct.Product
                        });
                    }
                }

                return orderViewModel;
            }

            return null;
        }
    }
}