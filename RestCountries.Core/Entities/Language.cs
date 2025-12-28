namespace RestCountries.Core.Entities;

public class Language
{
    protected Language() { /* For EF use */ }

    public Language(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}
