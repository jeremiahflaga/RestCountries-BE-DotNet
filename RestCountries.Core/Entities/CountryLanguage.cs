namespace RestCountries.Core.Entities;

// TODO: move to Data module, change visibility to internal
public class CountryLanguage
{
    public int CountryId { get; set; }
    public Country Country { get; protected set; }

    public int LanguageId { get; set; }
    public Language Language { get; protected set; }
}
