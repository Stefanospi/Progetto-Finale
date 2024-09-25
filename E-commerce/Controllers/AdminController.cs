using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_commerce.Controllers
{
    [Authorize(Roles = "admin")] // Autorizza solo gli admin
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IAuthService _authService;

        public AdminController(IOrderService orderService, IProductService productService, IAuthService authService)
        {
            _orderService = orderService;
            _productService = productService;
            _authService = authService;
        }

        // Dashboard generale
        public async Task<IActionResult> Dashboard()
        {
            // Recupera gli ordini, i prodotti e gli utenti
            var orders = await _orderService.GetAllOrdersAsync();
            var products = await _productService.GetAllProductsAsync();
            var users = await _authService.GetAllUsersAsync();
            // Crea una ViewModel che contiene i dati necessari
            var usersWithRoles = users.Select(user => new
            {
                user.IdUser,
                user.Username,
                user.Email,
                Roles = string.Join(", ", user.Roles.Select(r => r.Name)) // Prepara i ruoli come stringa
            }).ToList();

            // Passa i dati alla vista
            ViewBag.Users = usersWithRoles;

            // Passa i dati alla vista
            ViewBag.Orders = orders;
            ViewBag.Products = products;

            return View();
        }
    }
}
