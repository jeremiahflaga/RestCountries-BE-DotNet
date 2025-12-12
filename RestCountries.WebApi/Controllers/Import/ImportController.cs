using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.WebRequestMethods;

namespace RestCountries.WebApi.Controllers.Import;

[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;

    public ImportController(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task Import()
    {
        var httpClient = httpClientFactory.CreateClient("RestCountriesHttpClient");
        using var response = await httpClient.GetAsync("/v3.1/independent?status=true");

        response.EnsureSuccessStatusCode();
    }

    //// GET: api/<CountriesController>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<CountriesController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<CountriesController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<CountriesController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<CountriesController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
