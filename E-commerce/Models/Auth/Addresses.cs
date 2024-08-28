using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.Auth
{
    public class Addresses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }  // Opzionale

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        // Relazione con l'utente
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
