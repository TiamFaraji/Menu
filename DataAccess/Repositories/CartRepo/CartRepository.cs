using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.CartRepo
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(AppDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task Add(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var data = await GetById(id);
            _context.Carts.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Cart> GetAll(Expression<Func<Cart, bool>> Where = null)
        {
            var data = _context.Carts.AsQueryable();
            return data;
        }

        public async Task<Cart> GetById(int id)
        {
            return await _context.Carts
                .Where(c => c.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Cart cart)
        {
            try
            {
                _context.Update(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public IQueryable<CartItems> GetAllCartItems(Expression<Func<CartItems, bool>> Where = null)
        {
            var data = _context.CartItems.AsQueryable();
            return data;
        }

        public async Task DeleteCartItem(CartItems items)
        {
            _context.CartItems.Remove(items);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllCartItemsAsync(int userId)
        {
            var items = await _context.CartItems
                .Include(ci => ci.cart)
                .Where(ci => ci.cart.UserId == userId)
                .ToListAsync();
            _context.CartItems.RemoveRange(items);

            await _context.SaveChangesAsync();
        }


        public async Task AddCartItem(CartItems items)
        {
            _context.CartItems.Add(items);
            await _context.SaveChangesAsync();
        }

        public Task Update(CartItems existingItem)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCartItem(CartItems existingItem)
        {
            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<CartItems> entities)
        {
            _context.CartItems.RemoveRange(entities);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        Task ICartRepository.Update(Cart cart)
        {
            return Update(cart);
        }

        public async Task<bool> UpdateO(Cart cart)
        {
            try
            {
                var exists = await _context.Carts.AnyAsync(c => c.Id == cart.Id);
                if (!exists) return false;


                _context.Attach(cart);
                _context.Entry(cart).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart");
                return false;
            }
        }
    }

}


