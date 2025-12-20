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
    public DbSet<Language> Languages => Set<Language>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>()
            .Property(x => x.CCA2)
            .HasMaxLength(2);

        //modelBuilder.Entity<Country>(entity =>
        //{
        //    entity.OwnsMany(p => p.Languages, ownedNavigationBuilder =>
        //    {
        //        ownedNavigationBuilder.ToJson();
        //    });
        //});

        modelBuilder.Entity<Language>()
            .Property(x => x.Code)
            .HasMaxLength(3);

        modelBuilder.Entity<CountryLanguage>(x =>
        {
            x.ToTable("CountryLanguages");

            // Composite PK and name
            x.HasKey(sc => new { sc.CountryId, sc.LanguageId });

            x.HasOne(sc => sc.Country)
                .WithMany(s => s.CountryLanguages)
                .HasForeignKey(sc => sc.CountryId);

            x.HasOne(sc => sc.Language)
                .WithMany(s => s.CountryLanguages)
                .HasForeignKey(sc => sc.LanguageId);
        });
    }
}
