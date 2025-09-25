using System.Text.Json;
using RestSharp;

namespace BusBoard.Services;

public class ApiService(IRestClientWrapper client) : IApiService
{
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var request = new RestRequest(endpoint);
        return await GetAsync<T>(request);
    }
    public async Task<T> GetAsync<T>(RestRequest request)
    {
        T? response;

        try
        {
            response = await client.GetAsync<T>(request);
        }
        catch (JsonException exception)
        {
            throw new JsonException($"'GET {request.Resource}' Exception | Failed to Parse Response", exception);
        }
        catch (HttpRequestException exception)
        {
            throw new HttpRequestException($"'GET {request.Resource}' Exception | Request Failed with {exception.StatusCode}",
                exception);
        }

        if (response == null)
        {
            throw new HttpRequestException($"'GET {request.Resource}' Exception | Response was Null");
        }

        return response;
    }
}