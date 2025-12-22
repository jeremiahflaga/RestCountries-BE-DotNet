using System.ComponentModel.DataAnnotations;

namespace RestCountries.WebApi.Controllers.Countries;

public class GetCountriesInputDto
{
    public string? Region { get; set; }         // /countries?region=Asia
    public string? Name { get; set; }           // /countries?name=land
    public long? MinPopulation { get; set; }    // /countries?minPopulation=10000000
    public string? Sort { get; set; }           // /countries?sort=name or sort=-population
    [Required]
    public int? Page { get; set; }              // /countries?page=1
    [Required]
    public int? PageSize { get; set; }          // /countries?pageSize=20
    public string? Fields { get; set; }         // /countries?fields=name,population,capital
}

