using E_commerce.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.Order
{
    public class Orders
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

 
        public int? UserId { get; set; }
        public Users User { get; set; }
        // Aggiungiamo SessionId per utenti non loggati
        public string?SessionId { get; set; }  // Per gli utenti anonimi

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }  // e.g., Pending, Completed, Cancelled

        public int ShippingAddressId { get; set; }
        public Addresses ShippingAddress { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
