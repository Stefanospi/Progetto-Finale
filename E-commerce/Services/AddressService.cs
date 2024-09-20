using E_commerce.Context;
using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _ctx;
        private readonly ILogger<AddressService> _logger;

        public AddressService(DataContext ctx, ILogger<AddressService> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        // Aggiungi un nuovo indirizzo
        public async Task AddAddresses(Addresses addresses)
        {
            if (addresses == null)
            {
                _logger.LogWarning("Tentativo di aggiungere un indirizzo nullo.");
                throw new ArgumentNullException(nameof(addresses), "L'indirizzo non può essere nullo.");
            }

            _ctx.Addresses.Add(addresses);
            await _ctx.SaveChangesAsync();

            _logger.LogInformation($"Indirizzo aggiunto correttamente per l'utente o la sessione {addresses.UserId ?? addresses.SessionId}");
        }

        // Ottieni gli indirizzi per il SessionId (per utenti non autenticati)
        public async Task<IEnumerable<Addresses>> GetAddressesBySessionIdAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                _logger.LogWarning("SessionId nullo o vuoto passato al metodo GetAddressesBySessionIdAsync.");
                throw new ArgumentException("SessionId non valido.");
            }

            _logger.LogInformation($"Recupero indirizzi per la sessione: {sessionId}");

            return await _ctx.Addresses
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }

        // Ottieni gli indirizzi per UserId (per utenti autenticati)
        public async Task<IEnumerable<Addresses>> GetAddressesByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("UserId non valido passato al metodo GetAddressesByUserIdAsync.");
                throw new ArgumentException("UserId non valido.");
            }

            _logger.LogInformation($"Recupero indirizzi per l'utente con UserId: {userId}");

            return await _ctx.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
