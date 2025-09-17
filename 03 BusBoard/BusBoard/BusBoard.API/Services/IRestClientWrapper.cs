using RestSharp;

namespace BusBoard.Services;

public interface IRestClientWrapper
{
    Task<T?> GetAsync<T>(RestRequest request);
}