using Microsoft.EntityFrameworkCore;

namespace wpfBazyDanych1._0;

public class dbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wpf4g1;Trusted_Connection=True;");
        
    }
}