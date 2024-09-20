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

        public async Task<Products> CreateProductsAsync(Products products, IFormFile imageFile)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            // Controllo che il prodotto abbia un nome valido
            if (string.IsNullOrWhiteSpace(products.Name)) throw new ArgumentException("Il nome del prodotto è obbligatorio.");

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imageFile.CopyToAsync(ms);
                    products.Image = ms.ToArray();
                }
            }

            _ctx.Products.Add(products);
            await _ctx.SaveChangesAsync();
            return products;
        }

        public async Task<Products> DeleteProductAsync(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            if (product == null) throw new ArgumentException("Il prodotto non esiste.");

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
            if (categoryId <= 0) throw new ArgumentException("ID della categoria non valido.");

            return await _ctx.Products
                             .Where(p => p.CategoryId == categoryId)
                             .ToListAsync();
        }

        public async Task<Products> GetProductsById(int id)
        {
            if (id <= 0) throw new ArgumentException("ID prodotto non valido.");

            var product = await _ctx.Products
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) throw new ArgumentException("Il prodotto non esiste.");

            return product;
        }

        public async Task<Products> UpdateProductsAsync(Products products, IFormFile imageFile)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            var existingProduct = await _ctx.Products.FindAsync(products.ProductId);
            if (existingProduct == null) throw new ArgumentException("Il prodotto non esiste!");

            // Validazione del nome del prodotto
            if (string.IsNullOrWhiteSpace(products.Name)) throw new ArgumentException("Il nome del prodotto è obbligatorio.");

            existingProduct.Name = products.Name;
            existingProduct.Description = products.Description;
            existingProduct.Price = products.Price;
            existingProduct.StockQuantity = products.StockQuantity;
            existingProduct.Sizes = products.Sizes; // Aggiornamento delle taglie

            // Gestione dell'upload dell'immagine
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imageFile.CopyToAsync(ms);
                    existingProduct.Image = ms.ToArray();
                }
            }

            await _ctx.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<IEnumerable<Products>> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentException("La query di ricerca non può essere vuota.");

            return await _ctx.Products
                             .Where(p => p.Name.Contains(query))
                             .ToListAsync();
        }

        public async Task DecreaseProductStockAsync(int productId, int quantity)
        {
            var product = await _ctx.Products.FindAsync(productId);
            if (product == null) throw new ArgumentException("Prodotto non trovato!");

            if (product.StockQuantity < quantity)
            {
                throw new InvalidOperationException("Stock insufficiente!");
            }

            product.StockQuantity -= quantity;

            _ctx.Products.Update(product);
            await _ctx.SaveChangesAsync();
        }
    }
}
