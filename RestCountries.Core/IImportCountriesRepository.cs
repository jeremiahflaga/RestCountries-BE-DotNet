using RestCountries.Core;

namespace RestCountries.WebApi.Controllers.Import;

public interface IImportCountriesRepository
{
    Task BulkUpsertAsync(IEnumerable<Country> countries);
    //Task BulkUpsertAsync(IEnumerable<Language> languages);
}
