using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.Auth
{
    public class Addresses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAddress { get; set; }

        [Required(ErrorMessage = "Indirizzo 1 è obbligatorio")]
        [StringLength(100, ErrorMessage = "Indirizzo 1 può avere massimo 100 caratteri.")]
        public string AddressLine1 { get; set; }

        [StringLength(100, ErrorMessage = "Indirizzo 2 può avere massimo 100 caratteri.")]
        public string AddressLine2 { get; set; }  // Opzionale

        [Required(ErrorMessage = "Città è obbligatoria")]
        [StringLength(50, ErrorMessage = "Città può avere massimo 50 caratteri.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Stato è obbligatorio")]
        [StringLength(50, ErrorMessage = "Stato può avere massimo 50 caratteri.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Codice Postale è obbligatorio")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Il Codice Postale deve essere numerico.")]
        [StringLength(10, ErrorMessage = "Codice Postale può avere massimo 10 caratteri.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Paese è obbligatorio")]
        [StringLength(50, ErrorMessage = "Paese può avere massimo 50 caratteri.")]
        public string Country { get; set; }

        // Relazione con l'utente
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public Users User { get; set; }

        // Aggiunta di SessionId per gli utenti non autenticati
        [StringLength(255)]
        public string? SessionId { get; set; }  // Utilizzato per identificare gli indirizzi degli utenti non loggati
    }
}
