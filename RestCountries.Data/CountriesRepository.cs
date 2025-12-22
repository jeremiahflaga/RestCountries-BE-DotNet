using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestCountries.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Data;
public class CountriesRepository : ICountriesRepository
{
    private readonly CountriesDbContext dbContext;

    public CountriesRepository(CountriesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Country> GetAll(CountryQuery q)
    {
        var queryable = dbContext.Countries.AsQueryable();

        if (!string.IsNullOrEmpty(q.Region))
            queryable = queryable.Where(c => c.Region == q.Region || c.Subregion == q.Region);

        if (!string.IsNullOrEmpty(q.Name))
            queryable = queryable.Where(
                c => c.CommonName.Contains(q.Name) || c.OfficialName.Contains(q.Name));


        if (q.MinPopulation.HasValue)
            queryable = queryable.Where(c => c.Population >= q.MinPopulation);
        
        // Sorting
        // Using System.Linq.Dynamic.Core allows passing a string like "Name desc"
        if (!string.IsNullOrEmpty(q.Sort))
            queryable = queryable.OrderBy(q.Sort);
        
        // Pagination
        var totalItems = queryable.Count();
        var results = queryable
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize);

        return results.ToList();
    }

}
