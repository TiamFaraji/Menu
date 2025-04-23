using Admin.Models;
using Core.OrderService;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }


        public async Task<IActionResult> Index()
        {
            var data = await _orderService.GetAllOrders();
            var orderedData = data.OrderByDescending(o => o.Id).ToList();
            return View(orderedData);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var data = await _orderService.GetOrderById(id);
            return View(data);
        }


        [HttpPost]
        public async Task<IActionResult> SetStatusCT([FromBody] StatusDto model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid request data"
                });
            }

            try
            {
                var result = await _orderService.SetStatus(model.Id, model.State);

                if (!result)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to update order status"
                    });
                }

                return Ok(new
                {
                    success = true,
                    orderId = model.Id,
                    newStatus = model.State ? "Accepted" : "Rejected"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
