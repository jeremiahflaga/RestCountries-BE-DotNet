using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using RestCountries.Core;
using RestCountries.WebApi.Controllers.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        UpdateByProperties = new List<string> { "CCA2" } // default is PK
    };

    private static BulkConfig bulkConfigForLanguages = new BulkConfig
    {
        //SetOutputIdentity = true,      // get generated IDs for inserts
        //PreserveInsertOrder = true,    // keep list order if needed
        UpdateByProperties = new List<string> { "Code" }
    };

    public async Task BulkUpsertAsync(IEnumerable<Country> countries)
    {
        await dbContext.BulkInsertOrUpdateAsync(countries, bulkConfigForCountries);
    }

    public async Task BulkUpsertAsync(IEnumerable<Language> languages)
    {
        await dbContext.BulkInsertOrUpdateAsync(languages, bulkConfigForLanguages);
    }

    public async Task BulkUpsertAsync(IEnumerable<CountryLanguage> countryLanguages)
    {
        await dbContext.BulkInsertOrUpdateAsync(countryLanguages);
    }

    public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
    {
        return await dbContext.Languages.ToListAsync();
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await dbContext.Countries.ToListAsync();
    }
}
