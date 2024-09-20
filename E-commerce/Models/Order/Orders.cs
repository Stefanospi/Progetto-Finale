using E_commerce.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.Order
{
    public class Orders
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // ID Utente, reso nullable per supportare utenti anonimi
        public int? UserId { get; set; }
        public Users User { get; set; }

        // SessionId per gli utenti non autenticati
        public string? SessionId { get; set; }

        [Required(ErrorMessage = "La data dell'ordine è obbligatoria.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "L'importo totale è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "L'importo totale deve essere maggiore di zero.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Lo stato dell'ordine è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Lo stato dell'ordine non può superare i 50 caratteri.")]
        public string Status { get; set; }  // Esempi: Pending, Completed, Cancelled

        [Required(ErrorMessage = "L'indirizzo di spedizione è obbligatorio.")]
        public int ShippingAddressId { get; set; }
        public Addresses ShippingAddress { get; set; }

        // Collezione di elementi dell'ordine
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
