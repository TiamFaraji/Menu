using Core.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly CategoryService _categoryservice;

        public CategoriesController(CategoryService categoryservice)
        {
            _categoryservice = categoryservice;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _categoryservice.GetCategory());
        }
    }
}
