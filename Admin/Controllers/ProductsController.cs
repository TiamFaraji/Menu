using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;
using Core.ProductService;
using Core.CategoryService;

namespace Admin.Controllers
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

        
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryservice.GetCategory(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                await _productservice.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _categoryservice.GetCategory(), "Id", "Title", product.CategoryId);
            return View(product);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productservice.GetProductDtoById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryservice.GetCategory(), "Id", "Title", product.CategoryId);
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productservice.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _categoryservice.GetCategory(), "Id", "Title", product.CategoryId);
            return View(product);
        }

        
        public async Task<IActionResult> Delete(int? id)
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productservice.GetProductById(id);
            await _productservice.DeleteProduct(product);
            return RedirectToAction(nameof(Index));
        }

    }
}
