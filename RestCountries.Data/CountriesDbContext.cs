using Microsoft.EntityFrameworkCore;
using RestCountries.Core;

namespace RestCountries.Data;

public class CountriesDbContext : DbContext
{
    public CountriesDbContext(DbContextOptions<CountriesDbContext> options) 
        : base(options) 
    {
    }

    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>()
            .Property(x => x.CCA2)
            .HasMaxLength(2);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.OwnsMany(p => p.Languages, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });
    }
}
