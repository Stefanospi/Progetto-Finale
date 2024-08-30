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

            var categories = await _categoriesService.GetCategoriesAsync();
            if (categories == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore nel recupero delle categorie.");
            }

            var viewModel = new ProductEditViewModel
            {
                Product = product,
                Categories = categories
            };

            return View(viewModel);
        }

        // Metodo per gestire l'aggiornamento del prodotto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                await _productService.UpdateProductsAsync(viewModel.Product);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante l'aggiornamento: {ex.Message}");
                return View(viewModel);
            }
        }
    }
}
