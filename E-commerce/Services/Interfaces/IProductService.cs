using E_commerce.Models.AllProduct;

namespace E_commerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<Products>CreateProductsAsync(Products products, IFormFile image);
        Task<IEnumerable<Products>>GetProductsAsync();
        Task<Products> GetProductsById(int id);
        Task<Products> UpdateProductsAsync(Products products, IFormFile imageFile);
        Task<Products> DeleteProductAsync(int id);
        Task<IEnumerable<Products>> GetProductsByCategoryAsync(int categoryId);

    }
}
