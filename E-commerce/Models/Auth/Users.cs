using E_commerce.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net;

namespace E_commerce.Models.Auth
{
    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "La password può avere max 255 caratteri!")]
        [Required(ErrorMessage = "Inserisci una Password!")]
        public string PasswordHash { get; set; }  // Memorizza l'hash della password

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string PhoneNumber { get; set; }

        // Relazione con gli indirizzi
        public ICollection<Addresses> Addresses { get; set; } = new List<Addresses>();

        //RIFERIMENTI EF
        [Required]
        public required List<Roles> Roles { get; set; } = [];

        // Relazione con gli ordini
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
