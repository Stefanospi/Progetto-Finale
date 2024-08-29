using E_commerce.Models.AllProduct;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products)
        {
            await _productService.CreateProductsAsync(products);
            return RedirectToAction("Index");
        }
    }
}
