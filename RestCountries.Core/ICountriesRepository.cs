using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Core;

public interface ICountriesRepository
{
    IEnumerable<Country> GetAll(CountryQuery query);
}

public class CountryQuery
{
    public string? Region { get; set; }         // /countries?region=Asia
    public string? Name { get; set; }           // /countries?name=land
    public long? MinPopulation { get; set; }    // /countries?minPopulation=10000000
    public string? Sort { get; set; }           // /countries?sort=name or sort=-population
    public int Page { get; set; } = 1;          // /countries?page=1
    public int PageSize { get; set; } = 20;     // /countries?pageSize=20
}