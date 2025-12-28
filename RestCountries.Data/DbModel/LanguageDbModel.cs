using RestCountries.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.Data.DbModel;
internal class LanguageDbModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public ICollection<CountryLanguageDbModel>? CountryLanguages { get; set; }

    internal Language ToLanguageEntity()
    {
        return new Language(Code, Name);
    }

    internal static LanguageDbModel FromLanguageEntity(Language language)
    {
        return new LanguageDbModel
        {
            Code = language.Code,
            Name = language.Name
        };
    }
}
