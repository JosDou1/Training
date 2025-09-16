using BusBoard.Models;
using BusBoard.Services;
using Moq;
using RestSharp;

namespace BusBoard.Tests.Services;

public class ApiServiceTests
{
    [Fact]
    public async Task CheckApiServiceThrowsExceptionWhenResponseIsNull()
    {
        var mockRestClient = new Mock<IRestClientWrapper>();
        mockRestClient.Setup(client => client.GetAsync<Arrival>(It.IsAny<RestRequest>())).ReturnsAsync((Arrival?)null);
        
        var apiService = new ApiService(mockRestClient.Object);
        
        await Assert.ThrowsAsync<HttpRequestException>(() => apiService.GetAsync<Arrival?>(It.IsAny<string>()));
    }
}