using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;

namespace FinalProjectMVC.Areas.CustomerPanel.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationDbContext _context { get; }

        [HttpPost]
        public ActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<CartItem> cartItems = _context.CartItems.Where(item => item.CustomerId == userId).ToList();

            var totalPrice = cartItems.Sum(cartItem => cartItem.SellerProduct?.Price * cartItem.Count ?? 0);

            Order newOrder = new Order()
            {
                CustomerId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice
                //OrderItems = (ICollection<OrderItem>)cartItems,
            };
            _context.Orders.Add(newOrder);

            _context.SaveChanges();


            List<OrderItem> My_Order_Items = new List<OrderItem>();


            var domain = "https://localhost:5009/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"CustomerPanel/Cart/success?id={userId}",
                CancelUrl = domain + $"CustomerPanel/Cart/cancel?id={newOrder.OrderId}",
            };

            foreach (var item in cartItems)
            {
                My_Order_Items.Add
                (
                    new OrderItem
                    {
                        OrderId = newOrder.OrderId,
                        Count = item.Count,
                        Price = item.SellerProduct.Price,
                        SellerProductId = item.SellerProductId,
                        Status = OrderStatus.Pending
                    }
                );
                var sessionLineItem = new SessionLineItemOptions
                {

                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.SellerProduct?.Price) * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.SellerProduct?.Product?.Name,
                        }

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            _context.OrderItems.AddRange(My_Order_Items);
            _context.SaveChanges();


            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }
    }
}
