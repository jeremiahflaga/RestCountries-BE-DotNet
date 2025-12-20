//using RestCountries.Data;

namespace RestCountries.Core;

public class Language
{
    //protected Language() { /* For EF use */ }

    //public Language(string code, string name)
    //{
    //    Code = code;
    //    Name = name;
    //}

    //public int Id { get; protected set; }
    public string Code { get; set; }
    public string Name { get; set; }

    //public ICollection<CountryLanguage>? CountryLanguages { get; protected set; }
}
