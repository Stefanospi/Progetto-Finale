using E_commerce.Models.Auth;

namespace E_commerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Users> RegisterAsync(Users user);
        Task<Users> LoginAsync(Users user);
    }
}
