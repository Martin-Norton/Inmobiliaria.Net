using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
}
