namespace RestCountries.WebApi.Controllers.Import;

public class CountryDto
{
    public Name name { get; set; }
    public string region { get; set; }
    public string subregion { get; set; }
    public List<string> capital { get; set; }
    public int population { get; set; }
    public double area { get; set; }
    public Dictionary<string, string> languages { get; set; }
    public Flags flags { get; set; }
    public string cca2 { get; set; }
}

public class Flags
{
    public string png { get; set; }
    public string svg { get; set; }
}

//public class Languages
//{
//    public string eng { get; set; }
//}

public class Name
{
    public string common { get; set; }
    public string official { get; set; }
}

