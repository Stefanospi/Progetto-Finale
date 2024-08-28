using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E_commerce.Models.AllProduct;

namespace E_commerce.Models.Order
{
    public class OrderItems
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }  // Reflects the price at the time of order
    }
}