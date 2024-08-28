using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.Auth
{
    public class Roles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRole { get; set; }

        [Required(ErrorMessage = "Inserisci il nome per il nuovo ruolo!")]
        [StringLength(50)]
        public required string Name { get; set; }

        //RIFERIMENTI EF
        [Required]
        public List<Users> Users { get; set; } = [];
    }
}
