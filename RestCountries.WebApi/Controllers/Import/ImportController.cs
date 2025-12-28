using Microsoft.AspNetCore.Mvc;
using RestCountries.Core.Entities;
using RestCountries.Core.Services;
using System.Diagnostics;

namespace RestCountries.WebApi.Controllers.Import;

[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;
    //private readonly ImportService importService;
    private readonly IImportCountriesRepository importCountriesRepository;
    private readonly ILogger<ImportController> logger;

    public ImportController(
        IHttpClientFactory httpClientFactory, 
        //ImportService importService,
        IImportCountriesRepository importCountriesRepository,
        ILogger<ImportController> logger)
    {
        this.httpClientFactory = httpClientFactory;
        //this.importService = importService;
        this.importCountriesRepository = importCountriesRepository;
        this.logger = logger;
    }

    [HttpPost]
    public async Task Import()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var httpClient = httpClientFactory.CreateClient("RestCountriesHttpClient");
            using var response = await httpClient.GetAsync("/v3.1/independent?status=true");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Failed to fetch countries data. Status code: {response.StatusCode}");
                return;
            }

            var countriesDto = await response.Content.ReadFromJsonAsync<List<ImportCountryDto>>();
            var importStats = await BulkImportCountries(countriesDto);

            logger.LogInformation($"Languages - Inserted: {importStats.LanguagesInsertedCount}, Updated: {importStats.LanguagesUpdatedCount}");
            logger.LogInformation($"Countries - Inserted: {importStats.CountriesInsertedCount}, Updated: {importStats.CountriesUpdatedCount}");
            logger.LogInformation($"CountryLanguages - Inserted: {importStats.CountryLanguagesInsertedCount}, Updated: {importStats.CountryLanguagesUpdatedCount}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation($"Import completed in {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    private async Task<BulkUpsertStatsInfo> BulkImportCountries(List<ImportCountryDto>? countriesDto)
    {
        var countries = new List<Country>();
        foreach (var countryDto in countriesDto)
        {
            var country = new Country(countryDto.cca2)
            {
                OfficialName = countryDto.name?.official,
                Name = countryDto.name?.common,
                Region = countryDto.region,
                Subregion = countryDto.subregion,
                Capital = countryDto.capital != null && countryDto.capital.Count > 0 ? countryDto.capital[0] : null,
                Population = countryDto.population,
                Area = countryDto.area,
                Flag = !string.IsNullOrEmpty(countryDto.flags?.png) ? countryDto.flags.png : countryDto.flags?.svg,
                Languages = countryDto.languages
                            .DistinctBy(l => l.Key)
                            .Select(l => new Language(l.Key, l.Value))
                            .ToList()
            };

            countries.Add(country);
        }
        return await importCountriesRepository.BulkUpsertAsync(countries);
    }

    //private async Task<BulkUpsertStatsInfo> BulkImportLanguages(List<ImportCountryDto>? countriesDto)
    //{
    //    var languages = countriesDto?.SelectMany(c => c.languages ?? new Dictionary<string, string>())
    //        .DistinctBy(l => l.Key)
    //        .Select(l => new Language(Guid.NewGuid(), l.Key, l.Value))
    //        .ToList();
    //    return await importCountriesRepository.BulkUpsertAsync(languages);
    //}

    //private async Task<BulkUpsertStatsInfo> BulkImportCountryLanguages(List<ImportCountryDto>? countriesDto)
    //{
    //    var languages = await importCountriesRepository.GetAllLanguagesAsync();
    //    var countries = await importCountriesRepository.GetAllCountriesAsync();
    //    var countryLanguages = new List<CountryLanguage>();
    //    foreach (var country in countries)
    //    {
    //        var languagesDto = countriesDto.FirstOrDefault(c => c.cca2 == country.CCA2)?.languages;
    //        foreach (var langDto in languagesDto)
    //        {
    //            var lang = languages.FirstOrDefault(l => l.Code == langDto.Key);
    //            countryLanguages.Add(new CountryLanguage { CountryId = country.Id, LanguageId = lang.Id });
    //        }
    //    }
    //    return await importCountriesRepository.BulkUpsertAsync(countryLanguages);
    //}
}
