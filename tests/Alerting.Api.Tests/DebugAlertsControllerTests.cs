using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Alerting.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Alerting.Infrastructure;

namespace Alerting.Api.Tests
{
    public class DebugAlertsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public DebugAlertsControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            var configured = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AlertsTestDb"));
                });
            });

            _client = configured.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task Post_ReturnsBodyOnFailure()
        {
            var create = new CreateAlertRuleDto { Name = "Debug Alert", IsActive = true };
            var postResp = await _client.PostAsJsonAsync("/api/alerts", create);
            var body = await postResp.Content.ReadAsStringAsync();
            _output.WriteLine("ResponseStatus:" + postResp.StatusCode);
            _output.WriteLine("ResponseBody:" + body);
            Assert.True(postResp.IsSuccessStatusCode, body);
        }
    }
}
