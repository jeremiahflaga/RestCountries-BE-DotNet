using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestCountries.Core;
using RestCountries.WebApi.Controllers.Import;

namespace RestCountries.Data;

public class ImportCountriesRepository : IImportCountriesRepository
{
    private readonly CountriesDbContext dbContext;

    public ImportCountriesRepository(CountriesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private static BulkConfig bulkConfigForCountries = new BulkConfig
    {
        //SetOutputIdentity = true,      // get generated IDs for inserts
        //PreserveInsertOrder = true,    // keep list order if needed
        UpdateByProperties = new List<string> { "CCA2" }, // default is PK
        CalculateStats = true,
    };

    private static BulkConfig bulkConfigForLanguages = new BulkConfig
    {
        //SetOutputIdentity = true,      // get generated IDs for inserts
        //PreserveInsertOrder = true,    // keep list order if needed
        UpdateByProperties = new List<string> { "Code" },
        CalculateStats = true,
    };

    private static BulkConfig bulkConfigForCountryLanguages = new BulkConfig
    {
        CalculateStats = true,
    };

    public async Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<Country> countries)
    {
        await dbContext.BulkInsertOrUpdateAsync(countries, bulkConfigForCountries);
        return GetBulkUpsertStatsInfo(bulkConfigForCountryLanguages.StatsInfo);
    }

    public async Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<Language> languages)
    {
        await dbContext.BulkInsertOrUpdateAsync(languages, bulkConfigForLanguages);
        return GetBulkUpsertStatsInfo(bulkConfigForCountryLanguages.StatsInfo);
    }

    public async Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<CountryLanguage> countryLanguages)
    {
        await dbContext.BulkInsertOrUpdateAsync(countryLanguages, bulkConfigForCountryLanguages);
        return GetBulkUpsertStatsInfo(bulkConfigForCountryLanguages.StatsInfo);
    }

    public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
    {
        return await dbContext.Languages.ToListAsync();
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await dbContext.Countries.ToListAsync();
    }

    private BulkUpsertStatsInfo GetBulkUpsertStatsInfo(StatsInfo statsInfo)
    {
        return new BulkUpsertStatsInfo
        {
            InsertedCount = statsInfo.StatsNumberInserted,
            UpdatedCount = statsInfo.StatsNumberUpdated
        };
    }
}
