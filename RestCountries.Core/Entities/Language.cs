namespace RestCountries.Core.Entities;

public class Language
{
    protected Language() { /* For EF use */ }

    public Language(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public int Id { get; protected set; }
    public string Code { get; set; }
    public string Name { get; set; }

    // TODO: create db entity for Language in Data module,
    // TODO: delete this property
    public ICollection<CountryLanguage>? CountryLanguages { get; protected set; }
}
