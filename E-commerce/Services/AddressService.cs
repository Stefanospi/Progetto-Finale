using E_commerce.Context;
using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Addresses>> GetAddressesBySessionIdAsync(string sessionId)
        {
            return await _ctx.Addresses
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Addresses>> GetAddressesByUserIdAsync(int userId)
        {
            return await _ctx.Addresses
                .Where(a => a.UserId == userId)  // Filtro per l'utente specifico
                .ToListAsync();  // Recupera la lista di indirizzi
        }
    }
}
