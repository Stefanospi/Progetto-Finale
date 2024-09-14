using E_commerce.Context;
using E_commerce.Models.AllProduct;
using E_commerce.Models.Auth;
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
        public async Task<Products> CreateProductsAsync(Products products, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imageFile.CopyToAsync(ms);
                    products.Image = ms.ToArray();
                }
            }

            // Salvataggio prodotto
            _ctx.Products.Add(products);
            await _ctx.SaveChangesAsync();
            return products;
        }
        public async Task<Products> DeleteProductAsync(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            // Recupera tutti i prodotti dal database
            return await _ctx.Products
                                 .Include(p => p.Category) // Include eventuali relazioni se necessario
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _ctx.Products
                     .Where(p => p.CategoryId == categoryId)
                     .ToListAsync();
        }

        //Recupera il prodotto con quel singolo id
        public async Task<Products> GetProductsById(int id)
        {
            return await _ctx.Products
                .Include(p=> p.Category)
                .FirstOrDefaultAsync(p=>p.ProductId==id);
        }

        public async Task<Products> UpdateProductsAsync(Products products, IFormFile imageFile)
        {
            var product = await _ctx.Products.FindAsync(products.ProductId);

            if (product == null)
            {
                throw new ArgumentException("Il prodotto non esiste!");
            }

            product.Name = products.Name;
            product.Description = products.Description;
            product.Price = products.Price;
            product.StockQuantity = products.StockQuantity;
            product.Sizes = products.Sizes; // Aggiornamento delle taglie

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imageFile.CopyToAsync(ms);
                    product.Image = ms.ToArray();
                }
            }

            await _ctx.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<Products>> SearchProducts(string query)
        {
            return await _ctx.Products
                .Where(p=> p.Name.Contains(query))
                .ToListAsync();
        }
    }
}
