using RestSharp;

namespace BusBoard.Services;

public class RestClientWrapper(string baseUrl) : IRestClientWrapper
{
    private readonly RestClient _client = new(baseUrl);

    public Task<T?> GetAsync<T>(RestRequest request) => _client.GetAsync<T>(request);
}