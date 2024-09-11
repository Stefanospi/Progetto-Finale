using E_commerce.Models.Auth;

namespace E_commerce.Services.Interfaces
{
    public interface IAddressService
    {
        Task AddAddresses(Addresses addresses);
        Task<IEnumerable<Addresses>> GetAddressesByUserIdAsync(int userId);
        Task<IEnumerable<Addresses>> GetAddressesBySessionIdAsync(string sessionId);

    }
}
