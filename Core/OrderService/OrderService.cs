using Admin.Models;
using DataAccess.Enums;
using DataAccess.Models;
using DataAccess.Repositories.CartRepo;
using DataAccess.Repositories.ProductRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.OrderService
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IProductRepository productRepository, ICartRepository cartRepository, ILogger<OrderService> logger)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _logger = logger;
        }


        public async Task<bool> AddToCart(int productId, int qty, int userId)
        {

            if (qty <= 0)
                return false;


            var product = await _productRepository.GetById(productId);
            if (product == null)
                return false;


            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == Status.Created);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = Status.Created,
                    Created = DateTime.Now,
                    Address = "[Default Address]",
                    Phone = "[Default Phone]"
                };
                await _cartRepository.Add(cart);
            }


            var existingItem = cart.CartItems?
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {

                existingItem.Qty += qty;
                await _cartRepository.UpdateCartItem(existingItem);
            }
            else
            {

                await _cartRepository.AddCartItem(new CartItems
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Qty = qty,
                    Price = product.Price,
                    Created = DateTime.UtcNow
                });
            }

            return true;
        }


        public async Task<List<CartItems>> GetUserCart(int userId)
        {
            var carts = await _cartRepository.GetAllCartItems(a => a.cart.UserId == userId
            && a.cart.Status == Status.Created)
                .Include(a => a.cart)
                .Include(a => a.product).AsNoTracking().ToListAsync();

            return carts;
        }


        public async Task<bool> RemoveFromCart(int id)
        {
            var carts = await _cartRepository.GetAllCartItems(a => a.Id == id).FirstOrDefaultAsync();
            await _cartRepository.DeleteCartItem(carts);

            return true;
        }


        public async Task RemoveAllFromCart(int userId)
        {
            var activeCart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == Status.Created);

            if (activeCart?.CartItems != null)
            {
                _cartRepository.RemoveRange(activeCart.CartItems);
                await _cartRepository.SaveChangesAsync();
            }
        }


        public async Task<bool> Pay(string phone, string address, int userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == Status.Created);

            if (cart == null || !cart.CartItems.Any())
                return false;


            cart.Phone = phone;
            cart.Address = address;
            cart.Status = Status.Final;
            cart.Payed = DateTime.UtcNow;


            foreach (var item in cart.CartItems)
            {
                if (item.product != null)
                {
                    item.Price = item.product.Price;
                }
            }

            await _cartRepository.Update(cart);
            return true;
        }


        public async Task CreateNewCart(int userId)
        {
            var newCart = new Cart
            {
                UserId = userId,
                Status = Status.Created,
                Created = DateTime.UtcNow,
                Address = "Default Address",
                Phone = "Default Phone"
            };
            await _cartRepository.Add(newCart);
        }


        public async Task<List<CartItems>> GetActiveCartItems(int userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == Status.Created);

            return cart?.CartItems?.ToList() ?? new List<CartItems>();
        }

        public async Task<List<Cart>> GetUserOrders(int userId)
        {
            return await _cartRepository.GetAll()
                 .Where(c => c.UserId == userId &&
                   (c.Status == Status.Final ||
                    c.Status == Status.Accepted ||
                    c.Status == Status.Rejected))
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.product)
                .OrderByDescending(c => c.Payed)
                .ToListAsync();
        }


        public async Task<List<AdminOrderDto>> GetAllOrders()
        {
            var carts = await _cartRepository.GetAll(a => a.Status != Status.Created)
                .Include(a => a.user)
                .Include(a => a.CartItems)
                .ThenInclude(a => a.product)
                .Select(s => new AdminOrderDto()
                {
                    Address = s.Address,
                    Id = s.Id,
                    Phone = s.Phone,
                    Status = s.Status,
                    Payed = s.Payed,
                    UserId = s.UserId,
                    UserName = s.user.UserName,
                    Items = s.CartItems.Select(c => new AdminOrderItemDto()
                    {
                        ProductId = c.product.Id,
                        ProductName = c.product.Title,
                        Quantity = c.Qty,
                        Price = c.Price
                    }).ToList()
                })
                .AsNoTracking().ToListAsync();

            return carts;
        }


        public async Task<AdminOrderDto> GetOrderById(int id)
        {
           
            Console.WriteLine($"Searching for order ID: {id}");

            var cart = await _cartRepository.GetAll()
                .Where(a => a.Id == id)  
                .Include(a => a.user)
                .Include(a => a.CartItems)
                .ThenInclude(a => a.product)
                .Select(s => new AdminOrderDto()
                {
                    Address = s.Address,
                    Id = s.Id,
                    Phone = s.Phone,
                    Status = s.Status,
                    Payed = s.Payed,
                    UserId = s.UserId,
                    UserName = s.user.UserName,
                    Items = s.CartItems.Select(c => new AdminOrderItemDto()
                    {
                        ProductId = c.product.Id,
                        ProductName = c.product.Title,
                        Quantity = c.Qty,
                        Price = c.Price
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            Console.WriteLine(cart == null
                ? "No order found"
                : $"Found order ID: {cart.Id}");

            return cart;
        }


        public async Task<bool> SetStatus(int orderId, bool state)
        {
            try
            {
                var cart = await _cartRepository.GetAll()
                    .FirstOrDefaultAsync(c => c.Id == orderId);

                if (cart == null)
                {
                    _logger.LogWarning($"Order {orderId} not found");
                    return false;
                }

                cart.Status = state ? Status.Accepted : Status.Rejected;

                return await _cartRepository.UpdateO(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order {orderId}");
                return false;
            }
        }
    }
}


