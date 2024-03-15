using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Person)
                .WithOne()
                .HasForeignKey<Customer>(c => c.PersonId);
            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Receipts)
                .HasForeignKey(r => r.CustomerId);
            modelBuilder.Entity<ReceiptDetail>()
               .HasKey(rd => new { rd.ReceiptId, rd.ProductId });
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Receipt)
                .WithMany(r => r.ReceiptDetails)
                .HasForeignKey(rd => rd.ReceiptId);
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Product)
                .WithMany(p => p.ReceiptDetails)
                .HasForeignKey(rd => rd.ProductId);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

    }
}
