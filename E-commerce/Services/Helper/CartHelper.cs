using E_commerce.Services.Interfaces;
using System.Security.Claims;

namespace E_commerce.Services.Helper
{
    public class CartHelper
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartHelper(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetCartItemCountAsync(ClaimsPrincipal user)
        {
            int itemCount = 0;

            if (user.Identity.IsAuthenticated)
            {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                itemCount = await _cartService.GetCartItemCountAsync(userId);
            }
            else
            {
                var sessionId = _httpContextAccessor.HttpContext.Request.Cookies["SessionId"];
                if (sessionId != null)
                {
                    itemCount = await _cartService.GetCartItemCountBySessionAsync(sessionId);
                }
            }

            return itemCount;
        }
    }

}
