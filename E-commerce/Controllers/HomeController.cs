using E_commerce.Models;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoriesService _categoriesService;

        public HomeController(ILogger<HomeController> logger,IProductService productService,ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategoriesAsync();
            var products = await _productService.GetProductsAsync();

            ViewBag.Categories = categories;
            return View(products);
        }

        // Azione per filtrare i prodotti per categoria
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            var categories = await _categoriesService.GetCategoriesAsync();

            ViewBag.Categories = categories;
            return View("Index", products);  // Usa la stessa vista Index per mostrare i prodotti filtrati
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
