using E_commerce.Models.AllProduct;

namespace E_commerce.Models.ViewModel
{
    public class ProductEditViewModel
    {
        public Products Product { get; set; }
        public IEnumerable<Categories> Categories { get; set; }
    }
}
