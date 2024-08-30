using E_commerce.Models.AllProduct;
using E_commerce.Models.ViewModel;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create(Products products)
        {
            await _productService.CreateProductsAsync(products);
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
        public async Task<IActionResult> Edit(Products product)
        {

            await _productService.UpdateProductsAsync(product);
            return RedirectToAction("Index", "Home");
            

        }
    }
}
