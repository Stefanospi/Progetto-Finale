using E_commerce.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.ProductCart
{
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }
        public Users User { get; set; }

        public ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    }
}
