
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;


namespace Api.IntegrationTests;

public class AuthApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public AuthApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    private record AuthResponse(string AccessToken);

    [Fact]
    public async System.Threading.Tasks.Task Register_Login_Me_Works()
    {
        var email = "test@example.com";
        var password = "Password123!";

        var reg = await _client.PostAsJsonAsync("/api/auth/register", new { email, password });
        Assert.True(reg.StatusCode is HttpStatusCode.Created or HttpStatusCode.Conflict);

        var login = await _client.PostAsJsonAsync("/api/auth/login", new { email, password });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);

        var auth = await login.Content.ReadFromJsonAsync <AuthResponse>();
        Assert.False(string.IsNullOrWhiteSpace(auth?.AccessToken));

        _client.DefaultRequestHeaders.Authorization  = new AuthenticationHeaderValue("Bearer",auth!.AccessToken);

        var me = await _client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.OK, me.StatusCode);


    }


}