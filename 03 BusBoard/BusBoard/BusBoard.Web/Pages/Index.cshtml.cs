using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusBoard.Web.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string? Postcode { get; set; }

    public IActionResult? OnPost()
    {
        return null;
        // var result = CallApi(Query); // Call your API server-side
        // ViewData["Result"] = result;
        // return Page();
    }
}
