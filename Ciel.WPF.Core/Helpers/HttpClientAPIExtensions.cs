using Ciel.WPF.Core.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Ciel.WPF.Core.Helpers;

public static class HttpClientAPIExtensions
{
    public static HttpClient AddBearerAuthentication(this HttpClient client, string accessToken)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }

    public static async Task<T> GetAsAPIAsync<T>(this HttpClient client, string? requestUri)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        using var response = client.GetAsync(requestUri).Result;

        response.EnsureSuccessStatusCode();

        var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<T>>();

        return apiResult.Result;
    }

    public static async Task<T> PostAsAPIAsync<T>(this HttpClient client, string? requestUri, object value)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        using var response = await client.PostAsJsonAsync(requestUri, value);

        response.EnsureSuccessStatusCode();

        var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<T>>();

        return apiResult.Result;
    }
}