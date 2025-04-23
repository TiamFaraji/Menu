using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();

        Task<Product> GetById(int id);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(int id);
        Task Delete(Product product);
    }
}
