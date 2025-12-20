using RestCountries.Core;

namespace RestCountries.WebApi.Controllers.Import;

public interface IImportCountriesRepository
{
    Task BulkUpsertAsync(IEnumerable<Country> countries);
    Task BulkUpsertAsync(IEnumerable<Language> languages);
    Task BulkUpsertAsync(IEnumerable<CountryLanguage> countryLanguages);
    Task<IEnumerable<Language>> GetAllLanguagesAsync();
    Task<IEnumerable<Country>> GetAllCountriesAsync();
}
