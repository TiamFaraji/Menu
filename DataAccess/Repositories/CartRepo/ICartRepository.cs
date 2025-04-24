//using DataAccess.Models;
//using Microsoft.EntityFrameworkCore.Storage;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataAccess.Repositories.CartRepo
//{
//    public interface ICartRepository
//    {

//        IQueryable<Cart> GetAll(Expression<Func<Cart, bool>> where = null);

//        Task<Cart> GetById(int id);

//        Task Add(Cart cart);

//        Task Update(Cart cart);

//        Task<bool> UpdateO(Cart cart);

//        Task Delete(int id);

//        Task Delete(Cart cart);

//        IQueryable<CartItems> GetAllCartItems(Expression<Func<CartItems, bool>> where = null);

//        Task AddCartItem(CartItems item);

//        Task DeleteCartItem(CartItems item);

//        Task Update(CartItems existingItem);

//        Task UpdateCartItem(CartItems existingItem);

//        void RemoveRange(IEnumerable<CartItems> entities);

//        Task SaveChangesAsync();

//        Task DeleteAllCartItemsAsync(int userId);
//    }
//}


using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories.CartRepo
{
    public interface ICartRepository
    {
        IQueryable<Cart> GetAll(Expression<Func<Cart, bool>> where = null);
        Task Add(Cart cart);
        Task<bool> Update(Cart cart);
        IQueryable<CartItems> GetAllCartItems(Expression<Func<CartItems, bool>> where = null);
        Task AddCartItem(CartItems item);
        Task DeleteCartItem(CartItems item);
        Task UpdateCartItem(CartItems existingItem);
        void RemoveRange(IEnumerable<CartItems> entities);
        Task SaveChangesAsync();
    }
}