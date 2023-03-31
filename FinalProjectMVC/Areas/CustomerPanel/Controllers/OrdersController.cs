using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.Areas.CustomerPanel.ViewModel;
using System.ComponentModel.DataAnnotations;
using FinalProjectMVC.Areas.SellerPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Area("CustomerPanel")]
    [Authorize(Roles = "Customer")]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Order> orderRepo;
        private readonly IRepository<OrderItem> orderItemRepo;

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


            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = MapOrderModel(order);
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cancel")]
        public async Task<IActionResult> CancelOrder(int Id)
        {
            var orderItem = await orderItemRepo.GetDetailsAsync(Id);

            if (orderItem is null)
                return NotFound();

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

            return RedirectToAction(nameof(Details), new {id = orderItem.OrderId});
        }

        // GET: CustomerPanel/Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
        //    return View();
        //}

        //// POST: CustomerPanel/Orders/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,CustomerId,TotalPrice,OrderDate")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
        //    return View(order);
        //}

        //// GET: CustomerPanel/Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Orders == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
        //    return View(order);
        //}

        //// POST: CustomerPanel/Orders/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,TotalPrice,OrderDate")] Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.OrderId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
        //    return View(order);
        //}

        //// GET: CustomerPanel/Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Orders == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .Include(o => o.Customer)
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //// POST: CustomerPanel/Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Orders == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
        //    }
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order != null)
        //    {
        //        _context.Orders.Remove(order);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        private CustomerOrdersViewModel? MapOrderModel(Order order)
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
                        orderViewModel.OrderItems.Add(new()
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
