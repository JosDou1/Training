using BusBoard.Models;
using BusBoard.Models.API;
using BusBoard.Models.Domain;

namespace BusBoard.Services;

public class PostcodeClient(IApiService apiService)
{
    public async Task<Postcode> GetPostcodeDetails(string postcode)
    {
        var postcodeResponse = await apiService.GetAsync<PostcodeResponse>($"postcodes/{postcode}");

        var postcodeCoordinates =
            new Postcode(postcode, postcodeResponse.Result.Longitude, postcodeResponse.Result.Latitude);

        return postcodeCoordinates;
    }
}