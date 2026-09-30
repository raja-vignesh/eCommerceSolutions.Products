
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

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
    }
    
}
