using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestCountries.Core.Entities;
using RestCountries.Core.Services;
using System.Diagnostics;

namespace RestCountries.WebApi.Controllers.Countries;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository countriesRepository;
    private readonly ILogger<CountriesController> logger;

    public CountriesController(ICountriesRepository countriesRepository,
        ILogger<CountriesController> logger)
    {
        this.countriesRepository = countriesRepository;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult GetCountries([FromQuery] GetCountriesInputDto q)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var countries = countriesRepository.GetAll(new CountryQuery
            {
                Region = q.Region,
                Name = q.Name,
                MinPopulation = q.MinPopulation,
                Sort = q.Sort,
                Page = q.Page ?? 1,
                PageSize = q.PageSize ?? 20
            });

            // Data Shaping
            var fields = !string.IsNullOrEmpty(q.Fields) ? q.Fields : "name,population,capital,region";
            var shapedData = ShapeData(countries, q.Fields);
            return Ok(shapedData);
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation($"{nameof(GetCountries)} completed in {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    private static IEnumerable<IDictionary<string, object>> ShapeData(IEnumerable<Country> data, string? fields)
    {
        var fieldList = string.IsNullOrWhiteSpace(fields)
            ? null
            : fields.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.ToLowerInvariant())
                    .ToHashSet();

        foreach (var item in data)
        {
            var dict = new Dictionary<string, object>();
            void Add(string key, object? value)
            {
                if (fieldList is null || fieldList.Contains(key.ToLowerInvariant()))
                    dict[key] = value!;
            }
            Add("name", item.Name);
            Add("population", item.Population);
            Add("capital", item.Capital);
            Add("region", item.Region);

            yield return dict;
        }
    }
}
