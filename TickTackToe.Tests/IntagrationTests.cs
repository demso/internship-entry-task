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
        // Arrange
        var line = "{\n    \"player\": \"X\",\n    \"row\": 1,\n    \"column\": 0\n}";
        var content = new StringContent(
            JsonSerializer.Serialize(line),
            Encoding.UTF8,
            "application/json");

        // Act
        var response1 = await _client.GetAsync("/health");
        var response2 = await _client.GetAsync("/games");
        var response3 = await _client.PostAsync("/games", null);
        var response4 = await _client.GetAsync("/games/1");
        var response5 = await _client.PutAsync("/games/1/move", content);
        

        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.InternalServerError, response3.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response4.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response5.StatusCode);
    }
}
