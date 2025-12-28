using RestCountries.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Core.Services;
public class ImportService
{
    private readonly IImportCountriesRepository importCountriesRepository;

    public ImportService(IImportCountriesRepository importCountriesRepository)
    {
        this.importCountriesRepository = importCountriesRepository;
    }

    public async Task ImportCountriesAsync(IEnumerable<Country> countries)
    {
        await importCountriesRepository.BulkUpsertAsync(countries);
    }
}
