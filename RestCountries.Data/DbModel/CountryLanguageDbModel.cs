using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Data.DbModel;
internal class CountryLanguageDbModel
{
    public int CountryId { get; set; }
    public CountryDbModel Country { get; set; }

    public int LanguageId { get; set; }
    public LanguageDbModel Language { get; set; }
}
