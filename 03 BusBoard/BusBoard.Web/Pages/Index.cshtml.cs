using System.Collections.Immutable;
using BusBoard.Models.Domain;
using BusBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusBoard.Web.Pages;

public class IndexModel(BusArrivalService busArrivalService) : PageModel
{
    [BindProperty] public string? Postcode { get; set; }
    public ImmutableList<StopPoint>? BusStops { get; private set; }

    public async Task<IActionResult?> OnPost()
    {
        if (string.IsNullOrWhiteSpace(Postcode))
        {
            ModelState.AddModelError(nameof(Postcode), "Enter a valid postcode");
            return Page();
        }

        try
        {
            BusStops = await busArrivalService.GetArrivalsForPostcodeAsync(Postcode);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError(nameof(Postcode), ex.Message);
        }
        return Page(); 
    }
}
