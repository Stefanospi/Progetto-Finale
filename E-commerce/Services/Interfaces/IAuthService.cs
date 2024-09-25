using E_commerce.Models.Auth;

namespace E_commerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Users> RegisterAsync(Users user);
        Task<Users> LoginAsync(Users user);
        Task<Users> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(Users user);
        Task<List<Users>> GetAllUsersAsync();
    }
}
