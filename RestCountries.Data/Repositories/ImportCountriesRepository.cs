using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestCountries.Core.Entities;
using RestCountries.Core.Services;
using RestCountries.Data.DbModel;

namespace RestCountries.Data.Repositories;

public class ImportCountriesRepository : IImportCountriesRepository
{
    private readonly CountriesDbContext dbContext;

    public ImportCountriesRepository(CountriesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private static BulkConfig bulkConfigForCountries = new BulkConfig
    {
        UpdateByProperties = new List<string> { "CCA2" }, // default is PK
        //PropertiesToExcludeOnUpdate = new List<string> { "CountryLanguages" },
        CalculateStats = true,
    };

    private static BulkConfig bulkConfigForLanguages = new BulkConfig
    {
        UpdateByProperties = new List<string> { "Code" },
        //PropertiesToExcludeOnUpdate = new List<string> { "CountryLanguages" },
        CalculateStats = true,
    };

    private static BulkConfig bulkConfigForCountryLanguages = new BulkConfig
    {
        UpdateByProperties = new List<string> { "CountryId", "LanguageId" },
        CalculateStats = true,
    };

    public async Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<Country> countries)
    {
        var importLanguagesStats = await BulkImportLanguages(countries);
        var importCountriesStats = await BulkImportCountries(countries);
        var importCountryLanguagesStats = await BulkImportCountryLanguages(countries);

        return new BulkUpsertStatsInfo
        {
            CountriesInsertedCount = importCountriesStats.InsertedCount,
            CountriesUpdatedCount = importCountriesStats.UpdatedCount,
            LanguagesInsertedCount = importLanguagesStats.InsertedCount,
            LanguagesUpdatedCount = importLanguagesStats.UpdatedCount,
            CountryLanguagesInsertedCount = importCountryLanguagesStats.InsertedCount,
            CountryLanguagesUpdatedCount = importCountryLanguagesStats.UpdatedCount
        };
    }

    private async Task<DbBulkUpsertStatsInfo> BulkImportLanguages(IEnumerable<Country>? countries)
    {
        var languagesDbModel = countries?.SelectMany(c => c.Languages)
            .DistinctBy(l => l.Code)
            .Select(l => LanguageDbModel.FromLanguageEntity(l))
            .ToList();
        await dbContext.BulkInsertOrUpdateAsync(languagesDbModel, bulkConfigForLanguages);
        return GetBulkUpsertStatsInfo(bulkConfigForLanguages.StatsInfo);
    }

    private async Task<DbBulkUpsertStatsInfo> BulkImportCountries(IEnumerable<Country>? countries)
    {
        var countriesDbModel = new List<CountryDbModel>();
        foreach (var countryDto in countries)
        {
            var country = new CountryDbModel
            {
                Id = countryDto.Id,
                CCA2 = countryDto.CCA2,
                OfficialName = countryDto.OfficialName,
                Name = countryDto.Name,
                Region = countryDto.Region,
                Subregion = countryDto.Subregion,
                Capital = countryDto.Capital,
                Population = countryDto.Population,
                Area = countryDto.Area,
                Flag = countryDto.Flag,
            };

            countriesDbModel.Add(country);
        }
        await dbContext.BulkInsertOrUpdateAsync(countriesDbModel, bulkConfigForCountries);
        return GetBulkUpsertStatsInfo(bulkConfigForCountries.StatsInfo);
    }

    private async Task<DbBulkUpsertStatsInfo> BulkImportCountryLanguages(IEnumerable<Country>? countries)
    {
        try
        {

        var languagesDbModel = await dbContext.Languages.ToListAsync();
        var countriesDbModel = await dbContext.Countries.ToListAsync();
        var countryLanguagesDbModel = new List<CountryLanguageDbModel>();
        foreach (var countryDbModel in countriesDbModel)
        {
            var languages = countries.FirstOrDefault(c => c.CCA2 == countryDbModel.CCA2)?.Languages;
            foreach (var lang in languages)
            {
                var langDbModel = languagesDbModel.FirstOrDefault(l => l.Code == lang.Code);
                countryLanguagesDbModel.Add(new CountryLanguageDbModel 
                {
                    CountryId = countryDbModel.Id, 
                    LanguageId = langDbModel.Id 
                });
            }
        }

        await dbContext.BulkInsertOrUpdateAsync(countryLanguagesDbModel, bulkConfigForCountryLanguages);
        return GetBulkUpsertStatsInfo(bulkConfigForCountries.StatsInfo);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private DbBulkUpsertStatsInfo GetBulkUpsertStatsInfo(StatsInfo? statsInfo)
    {
        return new DbBulkUpsertStatsInfo
        {
            InsertedCount = statsInfo?.StatsNumberInserted ?? 0,
            UpdatedCount = statsInfo?.StatsNumberUpdated ?? 0
        };
    }
}

internal class DbBulkUpsertStatsInfo
{
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
}
