namespace BusBoard.Services;

public interface IApiService
{
    Task<T> GetAsync<T>(string endpoint);
}
