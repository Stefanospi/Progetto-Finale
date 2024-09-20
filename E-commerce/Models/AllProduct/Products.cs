using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.AllProduct
{
    public class Products
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Il nome del prodotto è obbligatorio.")]
        [StringLength(255, ErrorMessage = "Il nome del prodotto non può superare i 255 caratteri.")]
        public string Name { get; set; }

        // Campo opzionale per la descrizione
        [Required(ErrorMessage ="La descrizione del prodotto è obbligatoria.")]
        [StringLength(1000, ErrorMessage = "La descrizione del prodotto non può superare i 1000 caratteri.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La quantità in stock è obbligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantità deve essere un valore positivo.")]
        public int StockQuantity { get; set; }

        // Chiave esterna per la categoria
        [Required(ErrorMessage = "La categoria è obbligatoria.")]
        public int CategoryId { get; set; }

        // Navigazione per la categoria
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }

        // Campo per l'immagine opzionale
        public byte[]? Image { get; set; }

        // Proprietà per le taglie memorizzate come stringa separata da virgole
        [Required(ErrorMessage ="Le taglie del prodotto sono obbligatorie.")]
        [StringLength(50, ErrorMessage = "Le taglie non possono superare i 50 caratteri.")]
        public string Sizes { get; set; }  // Esempio: "S,M,L,XL" o "38,40,42"
    }
}
