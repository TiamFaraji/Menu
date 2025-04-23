using Core.CategoryService;
using Core.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productservice;
        private readonly CategoryService _categoryservice;

        public ProductsController(ProductService productservice, CategoryService categoryservice)
        {
            _productservice = productservice;
            _categoryservice = categoryservice;
        }


        public async Task<IActionResult> Index()
        {
            var data = await _productservice.GetProduct();
            return View(data);
        }



        public async Task<IActionResult> GetByCategory(int categoryId)
        {

            var currentCategory = await _categoryservice.GetCategoryById(categoryId);


            var otherCategories = await _categoryservice.GetOtherCategories(categoryId);


            var products = await _productservice.GetProductByCategory(categoryId);


            ViewBag.Category = currentCategory?.Title ?? "Products";
            ViewBag.OtherCategories = otherCategories;

            ViewBag.AllCategories = currentCategory != null
                ? otherCategories.Append(currentCategory).OrderBy(c => c.Title)
                : otherCategories;

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productservice.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
