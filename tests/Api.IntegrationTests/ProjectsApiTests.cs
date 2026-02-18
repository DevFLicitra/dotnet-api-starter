using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Api.IntegrationTests;

public sealed class ProjectsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async System.Threading.Tasks.Task Post_Project_Then_Get_List_Returns_Project()
    {
        var payload = new { name = "Integration Test Project" };

        var post = await _client.PostAsJsonAsync("/api/v1/projects", payload);
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);

        var get = await _client.GetAsync("/api/v1/projects?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var body = await get.Content.ReadAsStringAsync();
        Assert.Contains("Integration Test Project", body);
    }
}
