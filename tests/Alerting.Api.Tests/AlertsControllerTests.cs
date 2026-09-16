using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Alerting.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Alerting.Api.Tests
{
    public class AlertsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AlertsControllerTests(WebApplicationFactory<Program> factory)
        {
            var configured = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace ApplicationDbContext with InMemory for tests
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AlertsTestDb"));
                });
            });
            _client = configured.CreateClient();
        }

        [Fact]
        public async Task Post_Then_Get_ReturnsCreated()
        {
            var create = new CreateAlertRuleDto { Name = "Test Alert", IsActive = true };
            var postResp = await _client.PostAsJsonAsync("/api/alerts", create);
            Assert.Equal(HttpStatusCode.Created, postResp.StatusCode);

            var created = await postResp.Content.ReadFromJsonAsync<AlertRuleDto>();
            Assert.NotNull(created);

            var getResp = await _client.GetAsync($"/api/alerts/{created!.Id}");
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        }
    }
}
