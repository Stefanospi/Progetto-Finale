using E_commerce.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.ProductCart
{
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        public int? UserId { get; set; }  // Reso nullable per gestire gli utenti non autenticati
        [ForeignKey("UserId")]
        public Users? User { get; set; }

        public string ?SessionId { get; set; }  // Aggiunta per gli utenti non autenticati

        public ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    }

}
