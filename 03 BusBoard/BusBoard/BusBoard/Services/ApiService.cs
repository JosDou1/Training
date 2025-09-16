using System.Text.Json;
using RestSharp;

namespace BusBoard.Services;

public class ApiService(IRestClientWrapper client) : IApiService
{
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var request = new RestRequest(endpoint);

        T? response;

        try
        {
            response = await client.GetAsync<T>(request);
        }
        catch (JsonException exception)
        {
            throw new JsonException($"'GET {endpoint}' Exception | Failed to Parse Response", exception);
        }
        catch (HttpRequestException exception)
        {
            throw new HttpRequestException($"'GET {endpoint}' Exception | Request Failed with {exception.StatusCode}",
                exception);
        }

        if (response == null)
        {
            throw new HttpRequestException($"'GET {endpoint}' Exception | Response was Null");
        }

        return response;
    }
}