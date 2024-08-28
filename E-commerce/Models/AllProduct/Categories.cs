using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.AllProduct
{
    public class Categories
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public Categories ParentCategory { get; set; }

        public ICollection<Products> Products { get; set; } = new List<Products>();
    }
}
