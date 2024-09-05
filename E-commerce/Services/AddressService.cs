using E_commerce.Context;
using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _ctx;
        public AddressService(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddAddresses(Addresses addresses)
        {
            _ctx.Addresses.Add(addresses);
            await _ctx.SaveChangesAsync();
        }

    }
}
