using Core.FileUploadService;
using DataAccess.Models;
using DataAccess.Repositories.CategoryRepo;
using DataAccess.Repositories.ProductRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductService
{
    public class ProductService
    {
        private readonly IProductRepository _productrepository;
        private readonly IFileUploadService _fileuploadservice;
        public ProductService(IProductRepository productrepository, IFileUploadService fileuploadservice)
        {
            _productrepository = productrepository;
            _fileuploadservice = fileuploadservice;
        }

        public async Task<IEnumerable<Product>> GetProduct()
        {
            return await _productrepository.GetAll().Include(p => p.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(int categoryId)
        {
            return await _productrepository.GetAll().Where(p => p.CategoryId == categoryId).ToListAsync();

        }

        public async Task CreateProduct(ProductDto productDto)
        {
            var product = new Product()
            {
                CategoryId = productDto.CategoryId,
                Title = productDto.Title,
                Price = productDto.Price,
                Description = productDto.Description,
                Img = await _fileuploadservice.UploadFileAsync(productDto.Img)
            };

            await _productrepository.Add(product);
        }

        public async Task UpdateProduct(ProductDto productdto)
        {
            var product = await GetProductById(productdto.Id);
            product.Title = productdto.Title;
            product.Price = productdto.Price;   
            product.Description = productdto.Description;
            product.CategoryId = productdto.CategoryId;

            if(productdto.Img != null)
            {
                product.Img = await _fileuploadservice.UploadFileAsync(productdto.Img);
            }
            await _productrepository.Update(product);
        }

        public async Task DeleteProduct(Product product)
        {
            await _productrepository.Delete(product);
        }

        public async Task<Product> GetProductById(int Id)
        {
            return await _productrepository.GetById(Id);
        }

        public async Task<ProductDto> GetProductDtoById(int Id)
        {
            var product =await _productrepository.GetById(Id);
            var productDto = new ProductDto()
            {
                Id = Id,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ImgName = product.Img,
            };
            return productDto;
        }
    }
}

