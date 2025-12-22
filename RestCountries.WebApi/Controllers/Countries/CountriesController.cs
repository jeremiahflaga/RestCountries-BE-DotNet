using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestCountries.Core;

namespace RestCountries.WebApi.Controllers.Countries;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository countriesRepository;

    public CountriesController(ICountriesRepository countriesRepository)
    {
        this.countriesRepository = countriesRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries([FromQuery] GetCountriesInputDto q)
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
        if (!string.IsNullOrEmpty(q.Fields))
        {
            var shapedData = ShapeData(countries, q.Fields);
            return Ok(shapedData);
        }
        else
        {
            var shapedData = ShapeData(countries, "name,population,capital");
            return Ok(shapedData);
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
            Add("name", item.OfficialName);
            Add("population", item.Population);
            Add("capital", item.Capital);

            yield return dict;
        }
    }
}
