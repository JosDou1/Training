using RestSharp;

namespace BusBoard.Services;

public class RestClientWrapper(string baseUrl) : IRestClientWrapper
{
    private readonly RestClient client = new(baseUrl);

    public Task<T?> GetAsync<T>(RestRequest request) => client.GetAsync<T>(request);
}