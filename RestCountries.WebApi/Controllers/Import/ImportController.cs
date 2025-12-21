using Microsoft.AspNetCore.Mvc;
using RestCountries.Core;
using System.Diagnostics.Metrics;

namespace RestCountries.WebApi.Controllers.Import;

[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IImportCountriesRepository importCountriesRepository;

    public ImportController(IHttpClientFactory httpClientFactory, IImportCountriesRepository importCountriesRepository)
    {
        this.httpClientFactory = httpClientFactory;
        this.importCountriesRepository = importCountriesRepository;
    }

    [HttpPost]
    public async Task Import()
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("RestCountriesHttpClient");
            using var response = await httpClient.GetAsync("/v3.1/independent?status=true");

            var countriesDto = await response.Content.ReadFromJsonAsync<List<CountryDto>>();
            await BulkImportLanguages(countriesDto);
            await BulkImportCountries(countriesDto);
            await BulkImportCountryLanguages(countriesDto);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task BulkImportCountries(List<CountryDto>? countriesDto)
    {
        var countries = new List<Country>();
        foreach (var countryDto in countriesDto)
        {
            var country = new Country(countryDto.cca2)
            {
                OfficialName = countryDto.name?.official,
                CommonName = countryDto.name?.common,
                Region = countryDto.region,
                Subregion = countryDto.subregion,
                Capital = countryDto.capital != null && countryDto.capital.Count > 0 ? countryDto.capital[0] : null,
                Population = countryDto.population,
                Area = countryDto.area,
                //CountryLanguages = countryDto.languages.Select(x => new CountryLanguage { Code = x.Key, Name = x.Value }).ToList(),
                Flag = !string.IsNullOrEmpty(countryDto.flags?.png) ? countryDto.flags.png : countryDto.flags?.svg
            };

            countries.Add(country);
        }
        await importCountriesRepository.BulkUpsertAsync(countries);
    }

    private async Task BulkImportLanguages(List<CountryDto>? countriesDto)
    {
        var languages = countriesDto?.SelectMany(c => c.languages ?? new Dictionary<string, string>())
            .DistinctBy(l => l.Key)
            .Select(l => new Language(l.Key, l.Value))
            .ToList();
        await importCountriesRepository.BulkUpsertAsync(languages);
    }

    private async Task BulkImportCountryLanguages(List<CountryDto>? countriesDto)
    {
        var languages = await importCountriesRepository.GetAllLanguagesAsync();
        var countries = await importCountriesRepository.GetAllCountriesAsync();
        var countryLanguages = new List<CountryLanguage>();
        foreach (var country in countries)
        {
            var languagesDto = countriesDto.FirstOrDefault(c => c.cca2 == country.CCA2)?.languages;
            foreach (var langDto in languagesDto)
            {
                var lang = languages.FirstOrDefault(l => l.Code == langDto.Key);
                countryLanguages.Add(new CountryLanguage { CountryId = country.Id, LanguageId = lang.Id });
            }
        }
        await importCountriesRepository.BulkUpsertAsync(countryLanguages);
    }
}
