using E_commerce.Models.AllProduct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.ProductCart
{
    public class CartItems
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        [Required(ErrorMessage = "Il Carrello è obbligatorio.")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [Required(ErrorMessage = "Il Prodotto è obbligatorio.")]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [Required(ErrorMessage = "La quantità è obbligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere almeno 1.")]
        public int Quantity { get; set; }

        // Campo obbligatorio per la taglia selezionata
        [Required(ErrorMessage = "La taglia è obbligatoria.")]
        [StringLength(10, ErrorMessage = "La taglia non può superare i 10 caratteri.")]
        public string Size { get; set; }
    }
}
