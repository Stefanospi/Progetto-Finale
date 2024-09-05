using E_commerce.Models.Auth;

namespace E_commerce.Services.Interfaces
{
    public interface IAddressService
    {
        Task AddAddresses(Addresses addresses);
    }
}
