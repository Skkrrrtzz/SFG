using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEBTemplate.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public IndexModel(ILogger<IndexModel> logger,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContext = httpContextAccessor;
        }



        public async Task<IActionResult> OnGetAsync()
        {
            _httpContext.HttpContext.Session.Clear();
            return RedirectToPage("/Login");

        }
    }
}
