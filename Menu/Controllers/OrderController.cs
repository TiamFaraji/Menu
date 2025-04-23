using Core.OrderService;
using DataAccess.Models;
using Menu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Menu.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrderService> _logger;
        public OrderController(OrderService orderService, ILogger<OrderService> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Ok(new { res = false, msg = "youre not logged in" });
            }
            var res = await _orderService.AddToCart(model.productId, model.qty, Convert.ToInt32(userId));
            return Ok(new { res = true });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromBody] RemoveFromCartDto model)
        {
            var res = await _orderService.RemoveFromCart(model.Id);
            return Ok(new { res = true });
        }


        [HttpPost]
        public async Task<IActionResult> Pay(PayDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Phone) || string.IsNullOrWhiteSpace(model.Address))
            {
                TempData["ErrorMessage"] = "Address and Phone Number are required!";
                return RedirectToAction("Cart");
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0) return Unauthorized();

            try
            {
               
                var paymentSuccess = await _orderService.Pay(
                    model.Phone,
                    model.Address,
                    userId
                );

                if (!paymentSuccess)
                {
                    TempData["ErrorMessage"] = "Payment failed. Please try again.";
                    return RedirectToAction("Cart");
                }

               
                await _orderService.CreateNewCart(userId);

                return RedirectToAction("OrderPage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment error");
                TempData["ErrorMessage"] = "Payment processing error";
                return RedirectToAction("Cart");
            }
        }


        public async Task<IActionResult> Cart()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0) return Unauthorized();

            var cartItems = await _orderService.GetActiveCartItems(userId);

            return View(cartItems);
        }


        public async Task<IActionResult> OrderPage()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetUserOrders(userId);
            return View(orders);
        }


    }
}
