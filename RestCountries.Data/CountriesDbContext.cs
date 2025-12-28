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

        //modelBuilder.Entity<CountryDbModel>()
        //    .Property(x => x.Id)
        //    .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<CountryDbModel>()
            .Property(x => x.CCA2)
            .HasMaxLength(2);

        modelBuilder.Entity<LanguageDbModel>()
            .Property(x => x.Code)
            .HasMaxLength(3);

        modelBuilder.Entity<CountryLanguageDbModel>(x =>
        {
            x.ToTable("CountryLanguages");

            x.HasKey(x => new { x.CountryId, x.LanguageId });

            //x.Property(x => x.Country).HasColumnName("CountryId");
            //x.Property(x => x.LanguageId).HasColumnName("LanguageId");

            x.HasOne(x => x.Country)
                .WithMany(x => x.CountryLanguages)
                .HasForeignKey(x => x.CountryId);

            x.HasOne(x => x.Language)
                .WithMany(x => x.CountryLanguages)
                .HasForeignKey(x => x.LanguageId);
        });
    }
}
