using E_commerce.Models.AllProduct;
using E_commerce.Services.Helper;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoriesService _categoriesService;
        private readonly CartHelper _cartHelper;  // Aggiungi CartHelper

        public ProductController(IProductService productService, ICategoriesService categoriesService, CartHelper cartHelper)
        {
            _categoriesService = categoriesService;
            _productService = productService;
            _cartHelper = cartHelper;  // Inizializza CartHelper
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();
            return View();
        }
        [Authorize(Roles ="admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products product,IFormFile image)
        {

            await _productService.UpdateProductsAsync(product,image);
            return RedirectToAction("Index", "Home");
            

        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductsById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ottieni prodotti correlati basati sulla stessa categoria, escludendo il prodotto corrente
            var relatedProducts = await _productService.GetProductsByCategoryAsync(product.CategoryId);
            relatedProducts = relatedProducts.Where(p => p.ProductId != id).Take(4).ToList();

            ViewBag.RelatedProducts = relatedProducts;
            // Aggiorna il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);


            return View(product);
        }
        public async Task<IActionResult> Search(string query)
        {
            var filteredProducts = await _productService.SearchProducts(query);
            ViewBag.Categories = await _categoriesService.GetCategoriesAsync();

            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return View("/Views/Home/Index.cshtml", filteredProducts);
        }
    }
}
