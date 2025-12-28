using Microsoft.EntityFrameworkCore;
using RestCountries.Data.DbModel;

namespace RestCountries.Data;

public class CountriesDbContext : DbContext
{
    public CountriesDbContext(DbContextOptions<CountriesDbContext> options) 
        : base(options) 
    {
    }

    internal DbSet<CountryDbModel> Countries => Set<CountryDbModel>();
    internal DbSet<LanguageDbModel> Languages => Set<LanguageDbModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CountryDbModel>()
            .Property(x => x.CCA2)
            .HasMaxLength(2);

        modelBuilder.Entity<CountryDbModel>()
            .HasIndex(x => x.CCA2)
            .IsUnique();
        modelBuilder.Entity<CountryDbModel>()
            .HasIndex(b => b.Name);
        modelBuilder.Entity<CountryDbModel>()
            .HasIndex(b => b.Region);
        modelBuilder.Entity<CountryDbModel>()
            .HasIndex(b => b.Population);

        modelBuilder.Entity<LanguageDbModel>()
            .Property(x => x.Code)
            .HasMaxLength(3);

        modelBuilder.Entity<LanguageDbModel>()
            .HasIndex(b => b.Code)
            .IsUnique();

        modelBuilder.Entity<CountryLanguageDbModel>(x =>
        {
            x.ToTable("CountryLanguages");

            x.HasKey(x => new { x.CountryId, x.LanguageId });

            x.HasOne(x => x.Country)
                .WithMany(x => x.CountryLanguages)
                .HasForeignKey(x => x.CountryId);

            x.HasOne(x => x.Language)
                .WithMany(x => x.CountryLanguages)
                .HasForeignKey(x => x.LanguageId);
        });
    }
}
