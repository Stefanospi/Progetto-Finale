using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_commerce.Models.AllProduct;

namespace E_commerce.Models.Order
{
    public class OrderItems
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required(ErrorMessage = "L'ID dell'ordine è obbligatorio.")]
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [Required(ErrorMessage = "L'ID del prodotto è obbligatorio.")]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [Required(ErrorMessage = "La quantità è obbligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere almeno 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Il prezzo unitario è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo unitario deve essere maggiore di zero.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }  // Prezzo riflettente al momento dell'ordine
    }
}
