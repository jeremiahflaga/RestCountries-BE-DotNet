using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Core;
public class CountryLanguage
{
    public int CountryId { get; set; }
    public Country Country { get; protected set; }

    public int LanguageId { get; set; }
    public Language Language { get; protected set; }
}
