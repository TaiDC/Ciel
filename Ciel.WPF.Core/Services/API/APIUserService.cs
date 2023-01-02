using Ciel.WPF.Core.Contracts.Services.API;
using Ciel.WPF.Core.Helpers;
using Ciel.WPF.Core.Models;

namespace Ciel.WPF.Core.Services.API;

public class APIUserService : IAPIUserService
{
    private const string _apiServiceMe = "/api/User/info";
    private const string _apiServiceMePhoto = "/api/User/photo";

    private readonly HttpClient _httpClient;
    public APIUserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ciel");
    }

    public Task<User> GetUserInfoAsync(string accessToken)
    {
        return _httpClient.AddBearerAuthentication(accessToken)
                           .GetAsAPIAsync<User>(_apiServiceMe);
    }

    public Task<string> GetUserPhoto(string accessToken)
    {
        return _httpClient.AddBearerAuthentication(accessToken)
                            .GetAsAPIAsync<string>(_apiServiceMePhoto);
    }
}
