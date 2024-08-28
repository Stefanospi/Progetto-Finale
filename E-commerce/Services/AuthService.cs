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
            var userRole = await _ctx.Roles.Where(r => r.IdRole == 2).FirstOrDefaultAsync(); //1 = admin, 2 = user
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

    }
}
