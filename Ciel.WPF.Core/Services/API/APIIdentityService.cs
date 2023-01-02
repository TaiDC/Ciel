using Ciel.WPF.Core.Contracts.Services.API;
using Ciel.WPF.Core.Helpers;
using Ciel.WPF.Core.Models;
using System.Net.Http;

namespace Ciel.WPF.Core.Services.API;

public class APIIdentityService : IAPIIdentityService
{
    private const string _apiServiceLogin = "/api/Identity/login";

    private readonly HttpClient _httpClient;

    public APIIdentityService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ciel");
    }

    public Task<AuthenticationResult> LoginAsync(string username, string password)
    {
        var request = new {username, password };
        return _httpClient.PostAsAPIAsync<AuthenticationResult>(_apiServiceLogin, request);
    }
}
