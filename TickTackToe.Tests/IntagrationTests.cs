using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TickTackToe.Tests;

public class BasicIntegrationTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>> {
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task EndpointsExistTest()
    {
        Environment.SetEnvironmentVariable("BOARD_SIZE", "3");
        Environment.SetEnvironmentVariable("WIN_CONDITION", "3");

        // Act
        var response1 = await _client.GetAsync("/health");
        var response2 = await _client.GetAsync("/games");
        var response3 = await _client.PostAsync("/games", null);
        var response4 = await _client.GetAsync("/games/1");
        

        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response3.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response4.StatusCode);
    }
}
