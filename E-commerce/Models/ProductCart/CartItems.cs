using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E_commerce.Models.AllProduct;

namespace E_commerce.Models.ProductCart
{
    public class CartItems
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [Required]
        public int Quantity { get; set; }
        // Aggiungi questo campo per memorizzare la taglia
        [Required]
        public string Size { get; set; }
    }
}