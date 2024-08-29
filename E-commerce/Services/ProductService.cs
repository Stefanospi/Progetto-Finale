using E_commerce.Context;
using E_commerce.Models.AllProduct;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _ctx;
        public ProductService(DataContext dataContext)
        {
            _ctx = dataContext;
        }
        public async Task<Products> CreateProductsAsync(Products products)
        {
            _ctx.Products.Add(products);
            await _ctx.SaveChangesAsync();
            return products;
            
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            // Recupera tutti i prodotti dal database
            return await _ctx.Products
                                 .Include(p => p.Category) // Include eventuali relazioni se necessario
                                 .ToListAsync();
        }

        public Task<Products> UpdateProductsAsync(Products products)
        {
            throw new NotImplementedException();
        }
    }
}
