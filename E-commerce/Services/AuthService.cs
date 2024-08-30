using E_commerce.Context;
using E_commerce.Models.Auth;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _ctx;
        public AuthService(DataContext dataContext)
        {
            _ctx = dataContext;
        }

        public async Task<Users> RegisterAsync(Users user)
        {
            var existingUser = await _ctx.Users
     .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                // Username already exists
                throw new Exception("L'Username inserito è già in uso!");
            }
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
            var userRole = await _ctx.Roles.Where(r => r.IdRole == 1).FirstOrDefaultAsync(); //1 = admin, 2 = user
            user.Roles.Add(userRole);
            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public async Task<Users> LoginAsync(Users user)
        {
            string hashedPassword = PasswordHelper.HashPassword(user.PasswordHash);

            var existingUser = await _ctx.Users
                 .Include(u => u.Roles)
                 .Where(u => u.Username == user.Username && u.PasswordHash == hashedPassword)
                 .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                throw new Exception();
            }
            return existingUser;
        }
        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _ctx.Users.FindAsync(userId);
        }
        public async Task UpdateUserAsync(Users user)
        {
            var existingUser = await _ctx.Users.FindAsync(user.IdUser);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Aggiorna le proprietà
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash; // Considera di gestire la modifica della password in modo sicuro
            existingUser.BirthDate = user.BirthDate;
            existingUser.Gender = user.Gender;
            existingUser.PhoneNumber = user.PhoneNumber;

            // Aggiorna la password solo se è fornita e diversa dalla password attuale
            if (!string.IsNullOrEmpty(user.NewPassword))
            {
                existingUser.PasswordHash = PasswordHelper.HashPassword(user.NewPassword);
            }

            // Salva le modifiche nel database
            _ctx.Users.Update(existingUser);
            await _ctx.SaveChangesAsync();
        }

    }
}
