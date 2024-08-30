using E_commerce.Models.AllProduct;

namespace E_commerce.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task <IEnumerable<Categories>> GetCategoriesAsync();
    }
}
