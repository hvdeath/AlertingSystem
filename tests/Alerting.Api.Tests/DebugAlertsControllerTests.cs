using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Alerting.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Alerting.Api.Tests
{
    public class DebugAlertsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public DebugAlertsControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
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
