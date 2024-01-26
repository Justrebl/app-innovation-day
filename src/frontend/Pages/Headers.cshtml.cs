using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace CafeReadConf.Pages
{
    public class HeadersModel : PageModel
    {
        // Services DI 
        private readonly ILogger<HeadersModel> _logger;

        public HeadersModel() { }

        public async Task<IActionResult> OnGetAsync()
        {
            throw new Exception("Expected Exception to take a look at all the headers added by App Service Easy Auth");
        }
    }
}
