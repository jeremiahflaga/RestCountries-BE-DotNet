using Microsoft.AspNetCore.Mvc;
using RestCountries.Core;

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
        var httpClient = httpClientFactory.CreateClient("RestCountriesHttpClient");
        using var response = await httpClient.GetAsync("/v3.1/independent?status=true");

        var countriesDto = await response.Content.ReadFromJsonAsync<List<CountryDto>>();

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
                Languages = countryDto.languages.Select(x => new Language { Code = x.Key, Name = x.Value }).ToList(),
                Flag = !string.IsNullOrEmpty(countryDto.flags?.png) ? countryDto.flags.png : countryDto.flags?.svg
            };

            countries.Add(country);
        }
        await importCountriesRepository.BulkUpsertAsync(countries);

        response.EnsureSuccessStatusCode();
    }
}
