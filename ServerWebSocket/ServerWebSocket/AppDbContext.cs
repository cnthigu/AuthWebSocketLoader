using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ajuste a connection string para seu SQL Server local
        optionsBuilder.UseSqlServer("Server=localhost;Database=MeuBanco;Trusted_Connection=True;TrustServerCertificate=True");
    }
}