using Core.FileUploadService;
using Core.ProductService;
using DataAccess.Models;
using DataAccess.Repositories.CategoryRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CategoryService
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryrepository;
        private readonly IFileUploadService _fileuploadservice;
        public CategoryService(ICategoryRepository categoryrepository, IFileUploadService fileuploadservice)
        {
            _categoryrepository = categoryrepository;
            _fileuploadservice = fileuploadservice;
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            return await _categoryrepository.GetAll();
        }

        public async Task CreateCategory(CategoryDto categoryDto)
        {

            var category = new Category()
            {
                Id = categoryDto.Id,
                Title = categoryDto.Title,
                Img = await _fileuploadservice.UploadFileAsync(categoryDto.Img)
            };

            await _categoryrepository.Add(category);
        }

        public async Task UpdateCategory(CategoryDto categoryDto)
        {
            var category = await GetCategoryById(categoryDto.Id);
            category.Title = categoryDto.Title;

            if (categoryDto.Img != null)
            {
                category.Img = await _fileuploadservice.UploadFileAsync(categoryDto.Img);
            }
            await _categoryrepository.Update(category);
        }

        public async Task DeleteCategory(Category category)
        {
            await _categoryrepository.Delete(category);
        }

        public async Task<Category> GetCategoryById(int Id)
        {
            return await _categoryrepository.GetById(Id);
        }


        public async Task<IEnumerable<Category>> GetOtherCategories(int Id)
        {
            return await _categoryrepository.GetAllExcept(Id);
        }



    }
}
