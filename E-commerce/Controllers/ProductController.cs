using E_commerce.Models.AllProduct;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoriesService _categoriesService;
        public ProductController(IProductService productService, ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
            _productService = productService;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products,IFormFile image)
        {
            await _productService.CreateProductsAsync(products,image);
            return RedirectToAction("Index","Home");
        }

        // Metodo per visualizzare il modulo di modifica
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductsById(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();

            return View(product);
        }

        // Metodo per gestire l'aggiornamento del prodotto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products product,IFormFile image)
        {

            await _productService.UpdateProductsAsync(product,image);
            return RedirectToAction("Index", "Home");
            

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductsById(id);
            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public async Task<IActionResult> Search(string query)
        {
            var filteredProducts = await _productService.SearchProducts(query);
            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();
            return View("/Views/Home/Index.cshtml", filteredProducts);

        }

    }
}
