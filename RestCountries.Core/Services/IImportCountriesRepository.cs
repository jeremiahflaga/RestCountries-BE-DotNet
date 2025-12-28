using RestCountries.Core.Entities;

namespace RestCountries.Core.Services;

public interface IImportCountriesRepository
{
    Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<Country> countries);
    //Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<Language> languages);
    //Task<BulkUpsertStatsInfo> BulkUpsertAsync(IEnumerable<CountryLanguage> countryLanguages);
    //Task<IEnumerable<Language>> GetAllLanguagesAsync();
    //Task<IEnumerable<Country>> GetAllCountriesAsync();
}

public class BulkUpsertStatsInfo
{
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
}
