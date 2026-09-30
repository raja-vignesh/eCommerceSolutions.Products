
using Microsoft.EntityFrameworkCore;

namespace Products.Infra;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
}
