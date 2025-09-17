using RestSharp;

namespace BusBoard.Services;

public interface IApiService
{
    Task<T> GetAsync<T>(string endpoint);
    Task<T> GetAsync<T>(RestRequest request);
}