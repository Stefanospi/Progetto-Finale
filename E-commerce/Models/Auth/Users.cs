using E_commerce.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models.Auth
{
    public enum Gender
    {
        Maschio,
        Femmina,
        Altro
    }

    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il nome può avere massimo 50 caratteri.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il cognome può avere massimo 50 caratteri.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria.")]
        [StringLength(50, ErrorMessage = "L'email può avere massimo 50 caratteri.")]
        [EmailAddress(ErrorMessage = "Inserisci un'email valida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il nome utente è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il nome utente può avere massimo 50 caratteri.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "La password può avere massimo 255 caratteri.")]
        public string PasswordHash { get; set; }  // Memorizza l'hash della password

        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "La nuova password può avere massimo 255 caratteri.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "La data di nascita è obbligatoria.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Il genere è obbligatorio.")]
        public required Gender Gender { get; set; }

        [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
        [StringLength(15, ErrorMessage = "Il numero di telefono può avere massimo 15 caratteri.")]
        [Phone(ErrorMessage = "Inserisci un numero di telefono valido.")]
        public string PhoneNumber { get; set; }

        // Relazione con gli indirizzi
        public ICollection<Addresses> Addresses { get; set; } = new List<Addresses>();

        // Riferimenti EF per i ruoli
        [Required(ErrorMessage = "Almeno un ruolo deve essere assegnato.")]
        public required List<Roles> Roles { get; set; } = [];

        // Relazione con gli ordini
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}
