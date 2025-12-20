using Microsoft.AspNetCore.Mvc;
using RestCountries.Core;
using RestCountries.Data;
using System;
using static System.Net.WebRequestMethods;

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

        //var languages = countriesDto?.SelectMany(c => c.languages ?? new Dictionary<string, string>())
        //    .DistinctBy(l => l.Key)
        //    .Select(l => new Language(l.Key, l.Value))
        //    .ToList();
        //await importCountriesRepository.BulkUpsertAsync(languages);

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
        //var countries = countriesDto.Select(countryDto => );
        await importCountriesRepository.BulkUpsertAsync(countries);



        //foreach (var countryDto in countries)
        //{
        //    await importCountriesRepository.UpsertAsync();
        //}

        response.EnsureSuccessStatusCode();
    }

    //// GET: api/<CountriesController>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<CountriesController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<CountriesController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<CountriesController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<CountriesController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
