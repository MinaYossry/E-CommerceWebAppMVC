using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using FinalProjectMVC.RepositoryPattern;
using FinalProjectMVC.Areas.SellerPanel.ViewModel;

namespace FinalProjectMVC.Areas.SellerPanel.Controllers
{
    [Area("SellerPanel")]
    [Authorize(Roles = "Seller")]
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<OrderItem> orderItemsRepo;

        public OrderItemsController(ApplicationDbContext context, IRepository<OrderItem> orderItemsRepo)
        {
            _context = context;
            this.orderItemsRepo = orderItemsRepo;
        }

        // GET: SellerPanel/OrderItems
        public async Task<IActionResult> Index()
        {
            var SellerOrders = (await orderItemsRepo.FilterAsync(o => o.SellerProduct?.SellerId == User.GetUserId())).OrderBy(o => o.Status).ToList();

            var viewModel = new List<SellerOrderItemViewModel>();

            foreach (var orderItem in SellerOrders)
            {
                viewModel.Add(new()
                {
                    Id = orderItem.Id,
                    Status = orderItem.Status,
                    Count = orderItem.Count,
                    Price = orderItem.Price,
                    OrderDate = orderItem.Order.OrderDate,
                    Customer = orderItem.Order.Customer,
                    Address = orderItem.Order.Address,
                    Product = orderItem.SellerProduct.Product
                });
            }

            

            return View(viewModel);
        }

        // GET: SellerPanel/OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.SellerProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: SellerPanel/OrderItems/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId");
            ViewData["SellerProductId"] = new SelectList(_context.SellerProducts, "Id", "Id");
            return View();
        }

        // POST: SellerPanel/OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,OrderId,SellerProductId,Count,Price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", orderItem.OrderId);
            ViewData["SellerProductId"] = new SelectList(_context.SellerProducts, "Id", "Id", orderItem.SellerProductId);
            return View(orderItem);
        }

        // GET: SellerPanel/OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", orderItem.OrderId);
            ViewData["SellerProductId"] = new SelectList(_context.SellerProducts, "Id", "Id", orderItem.SellerProductId);
            return View(orderItem);
        }

        // POST: SellerPanel/OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,OrderId,SellerProductId,Count,Price")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", orderItem.OrderId);
            ViewData["SellerProductId"] = new SelectList(_context.SellerProducts, "Id", "Id", orderItem.SellerProductId);
            return View(orderItem);
        }

        // GET: SellerPanel/OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.SellerProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: SellerPanel/OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderItems'  is null.");
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
          return (_context.OrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
