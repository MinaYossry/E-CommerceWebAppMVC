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
        private readonly IRepository<OrderItem> orderItemsRepo;

        public OrderItemsController( IRepository<OrderItem> orderItemsRepo)
        {
            this.orderItemsRepo = orderItemsRepo;
        }

        // GET: SellerPanel/OrderItems
        public async Task<IActionResult> Index()
        {
            var SellerOrders = (await orderItemsRepo.FilterAsync(o => o.SellerProduct?.SellerId == User.GetUserId())).OrderBy(o => o.Status).ToList();

            var viewModel = new List<SellerOrderItemViewModel>();

            foreach (var orderItem in SellerOrders)
            {
                if (orderItem.Order?.Customer is not null &&
                    orderItem.Order?.Address is not null &&
                    orderItem.SellerProduct?.Product is not null)
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


        public async Task<IActionResult> UpdateOrderStatus(int? Id, OrderStatus orderStatus)
        {
            var orderItem = await orderItemsRepo.GetDetailsAsync(Id);

            if (orderItem is null)
                return NotFound();

            if (orderItem.Status == OrderStatus.Delivered)
                ModelState.AddModelError("", "Sorry order has already been delivered");

            if (orderItem.Status >= orderStatus)
                ModelState.AddModelError("", "Sorry Can't modify the status to previous step");
            else
                orderItem.Status = orderStatus;

            if (ModelState.IsValid)
            {
                try
                {
                    await orderItemsRepo.UpdateAsync(Id, orderItem);
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
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CancelOrder(int? id)
        {
            return  RedirectToAction( nameof(UpdateOrderStatus), new { Id = id, orderStatus = OrderStatus.Cancelled });
        }

       

        private bool OrderItemExists(int id)
        {
            return orderItemsRepo.GetDetails(id) is not null;
        }
    }
}
