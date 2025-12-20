namespace RestCountries.Core;

public class Country
{
    protected Country() { /* For EF use */ }

    public Country(string cca2)
    {
        CCA2 = cca2;
        //CountryLanguages = new List<CountryLanguage>();
    }

    public int Id { get; protected set; }
    public string CCA2 { get; set; }
    public string? OfficialName { get; set; }
    public string? CommonName { get; set; }
    public string? Region { get; set; }
    public string? Subregion { get; set; }
    public string? Capital { get; set; }
    public int? Population { get; set; }
    public double? Area { get; set; }
    //public Dictionary<string, string>? Languages { get; set; }
    public List<Language> Languages { get; set; }
    public string? Flag { get; set; }
}
