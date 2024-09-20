using E_commerce.Context;
using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_commerce.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _ctx;
        private readonly ILogger<AuthService> _logger;

        public AuthService(DataContext dataContext, ILogger<AuthService> logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        // Registrazione utente
        public async Task<Users> RegisterAsync(Users user)
        {
            // Controlla se l'username esiste già
            var existingUser = await _ctx.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                _logger.LogWarning($"Tentativo di registrazione con username già in uso: {user.Username}");
                throw new Exception("L'Username inserito è già in uso!");
            }

            // Hash della password
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);

            // Assegna il ruolo di default (es. User)
            var userRole = await _ctx.Roles.FirstOrDefaultAsync(r => r.IdRole == 2); // 2 = user role
            if (userRole == null)
            {
                _logger.LogError("Ruolo utente non trovato.");
                throw new Exception("Ruolo utente non trovato.");
            }

            user.Roles.Add(userRole);
            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();

            _logger.LogInformation($"Nuovo utente registrato: {user.Username}");
            return user;
        }

        // Login utente
        public async Task<Users> LoginAsync(Users user)
        {
            string hashedPassword = PasswordHelper.HashPassword(user.PasswordHash);

            // Trova l'utente con la password hashata
            var existingUser = await _ctx.Users
                .Include(u => u.Roles)
                .Where(u => u.Username == user.Username && u.PasswordHash == hashedPassword)
                .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                _logger.LogWarning($"Tentativo di accesso fallito per username: {user.Username}");
                throw new Exception("Username o password non validi.");
            }

            _logger.LogInformation($"Accesso riuscito per utente: {user.Username}");
            return existingUser;
        }

        // Recupera utente per Id
        public async Task<Users> GetUserByIdAsync(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.LogWarning($"Utente con ID {userId} non trovato.");
                throw new Exception("Utente non trovato.");
            }

            return user;
        }

        // Aggiorna utente
        public async Task UpdateUserAsync(Users user)
        {
            var existingUser = await _ctx.Users.FindAsync(user.IdUser);

            if (existingUser == null)
            {
                _logger.LogWarning($"Tentativo di aggiornamento fallito per utente con ID {user.IdUser}.");
                throw new Exception("Utente non trovato.");
            }

            // Aggiorna le proprietà
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.BirthDate = user.BirthDate;
            existingUser.Gender = user.Gender;
            existingUser.PhoneNumber = user.PhoneNumber;

            // Aggiorna la password solo se fornita e diversa dalla precedente
            if (!string.IsNullOrEmpty(user.NewPassword))
            {
                existingUser.PasswordHash = PasswordHelper.HashPassword(user.NewPassword);
                _logger.LogInformation($"Password aggiornata per utente con ID {user.IdUser}.");
            }

            _ctx.Users.Update(existingUser);
            await _ctx.SaveChangesAsync();

            _logger.LogInformation($"Profilo aggiornato per utente con ID {user.IdUser}.");
        }
    }
}
