using E_commerce.Context;
using E_commerce.Models.AllProduct;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _ctx;
        public CategoriesService(DataContext ctx) { _ctx = ctx; }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await _ctx.Categories
                .ToListAsync();
        }
    }
}
