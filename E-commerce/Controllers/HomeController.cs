using E_commerce.Models;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoriesService categoriesService, ICartService cartService)
        {
            _categoriesService = categoriesService;
            _productService = productService;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategoriesAsync();
            var products = await _productService.GetProductsAsync();

            ViewBag.Categories = categories;

            // Imposta il conteggio degli articoli nel carrello
            await SetCartItemCountAsync();

            return View(products);
        }

        // Azione per filtrare i prodotti per categoria
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            var categories = await _categoriesService.GetCategoriesAsync();

            ViewBag.Categories = categories;

            // Imposta il conteggio degli articoli nel carrello
            await SetCartItemCountAsync();

            return View("Index", products);
        }

        private async Task SetCartItemCountAsync()
        {
            int itemCount = 0;

            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                itemCount = await _cartService.GetCartItemCountAsync(userId);
            }
            else
            {
                var sessionId = Request.Cookies["SessionId"];
                if (sessionId != null)
                {
                    itemCount = await _cartService.GetCartItemCountBySessionAsync(sessionId);
                }
            }

            ViewBag.CartItemCount = itemCount;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
