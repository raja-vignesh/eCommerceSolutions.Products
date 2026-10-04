
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using System.Diagnostics.Metrics;

namespace Products.Infra;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        modelBuilder.Entity<Product>()
         .Property(b => b.UnitPrice)
         .HasPrecision(10, 2);
        modelBuilder.Entity<Product>()
         .HasIndex(c => c.ProductName)
         .IsUnique();
        modelBuilder.Entity<Product>()
         .Property(p => p.ProductName)
         .HasMaxLength(50);
       
    }
    
}
