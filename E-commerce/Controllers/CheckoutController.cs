using E_commerce.Models.Auth;
using E_commerce.Models.Order;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAddressService _addressService;

        public CheckoutController(ICartService cartService,IAddressService addressService)
        {
            _cartService = cartService;
            _addressService = addressService;
        }

        public ActionResult AddAddress()
        {
            return View();
        }

        // Passaggio 1: Gestisce il salvataggio del nuovo indirizzo
        [HttpPost]
        public async Task<IActionResult> AddAddress(Addresses address)
        {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                address.UserId = userId;  // Associa l'indirizzo all'utente autenticato
                await _addressService.AddAddresses(address);

                // Dopo aver salvato l'indirizzo, reindirizza alla selezione dell'indirizzo per l'ordine
                return RedirectToAction("Index","Home");
            

        }


    }
}
