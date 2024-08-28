using E_commerce.Models.AllProduct;
using E_commerce.Models.Auth;
using E_commerce.Models.Order;
using E_commerce.Models.ProductCart;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Context
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }

        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la relazione tra l'entità 'Orders' e l'entità 'Users'
            modelBuilder.Entity<Orders>()
                // Specifica che ogni ordine ('Orders') è associato a un utente ('User')
                .HasOne(o => o.User)
                // Specifica che un utente può avere molti ordini
                .WithMany(u => u.Orders)
                // Imposta la chiave esterna nella tabella 'Orders' che punta alla tabella 'Users'
                .HasForeignKey(o => o.UserId)
                // Configura il comportamento quando viene eliminato un utente
                .OnDelete(DeleteBehavior.Restrict);  // Evita l'eliminazione a cascata degli ordini quando viene eliminato l'utente

            // Chiama il metodo base per garantire che qualsiasi configurazione predefinita o comportamenti siano applicati
            base.OnModelCreating(modelBuilder);
        }





    }
}
