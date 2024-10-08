﻿using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using E_commerce.Services;
using E_commerce.Services.Helper;

namespace E_commerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly CartHelper _cartHelper;

        private readonly IAuthService _authSvc;
        private readonly IOrderService _orderService;
        public AuthController(IAuthService authService,CartHelper cartHelper,IOrderService orderService)
        {
            _authSvc = authService;
            _cartHelper = cartHelper;
            _orderService = orderService;
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            // Recupera l'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _authSvc.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Recupera gli ordini dell'utente
            var orders = await _orderService.GetOrdersByUserIdAsync(user.IdUser);

            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            // Passa gli ordini alla vista
            ViewBag.Orders = orders;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users u)
        {
            try
            {
                await _authSvc.RegisterAsync(u);
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RegisterError", ex.Message);
                return View(u);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Users user)
        {
            try
            {
                var existingUser = await _authSvc.LoginAsync(user);
                if (existingUser == null)
                {
                    ModelState.AddModelError("LoginError", "");
                    return View(user);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim(ClaimTypes.NameIdentifier, existingUser.IdUser.ToString())
                };

                existingUser.Roles.ForEach(r =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, r.Name));
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", "");
                return View(user);
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Visualizza il modulo per modificare il profilo
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            // Recupera l'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _authSvc.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Users user)
        {

                // Solo aggiorna la password se è stata fornita
                if (!string.IsNullOrEmpty(user.NewPassword))
                {
                    // Considera di aggiungere ulteriori validazioni per la nuova password
                    user.PasswordHash = PasswordHelper.HashPassword(user.NewPassword);
                }

                await _authSvc.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");

        }
    }
}
