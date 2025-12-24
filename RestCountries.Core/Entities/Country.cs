namespace RestCountries.Core.Entities;

public class Country
{
    protected Country() { /* For EF use */ }

    public Country(string cca2)
    {
        CCA2 = cca2;
    }

    public int Id { get; protected set; }
    public string CCA2 { get; set; }
    public string? OfficialName { get; set; }
    public string? Name { get; set; }
    public string? Region { get; set; }
    public string? Subregion { get; set; }
    public string? Capital { get; set; }
    public int? Population { get; set; }
    public double? Area { get; set; }
    public string? Flag { get; set; }

    // TODO: create db entity for Country in Data module,
    // TODO: replace this property with list of languages
    public ICollection<CountryLanguage>? CountryLanguages { get; set; }
}
