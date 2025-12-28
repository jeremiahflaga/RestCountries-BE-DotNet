using RestCountries.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Data.DbModel;
internal class CountryDbModel
{
    public int Id { get; set; }
    public string CCA2 { get; set; }
    public string? OfficialName { get; set; }
    public string? Name { get; set; }
    public string? Region { get; set; }
    public string? Subregion { get; set; }
    public string? Capital { get; set; }
    public int? Population { get; set; }
    public double? Area { get; set; }
    public string? Flag { get; set; }

    public ICollection<CountryLanguageDbModel>? CountryLanguages { get; set; }

    public Country ToCountryEntity()
    {
        var country = new Country(CCA2)
        {
            OfficialName = OfficialName,
            Name = Name,
            Region = Region,
            Subregion = Subregion,
            Capital = Capital,
            Population = Population,
            Area = Area,
            Flag = Flag,
            Languages = CountryLanguages?
                .Select(cl => cl.Language.ToLanguageEntity())

        };
        return country;
    }
}
