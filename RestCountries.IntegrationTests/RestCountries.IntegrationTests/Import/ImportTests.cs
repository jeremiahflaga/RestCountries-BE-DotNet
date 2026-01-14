using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCountries.IntegrationTests.Import;
public class ImportTests
{
    [ClassDataSource<WebApplicationFactory>(Shared = SharedType.PerTestSession)]
    public required WebApplicationFactory WebApplicationFactory { get; init; }

    [Test]
    public async Task Import_successful()
    {
        var client = WebApplicationFactory.CreateClient();

        var response = await client.PostAsync("/api/Import", null);

        await Assert.That(response.IsSuccessStatusCode).IsTrue();
    }
}
