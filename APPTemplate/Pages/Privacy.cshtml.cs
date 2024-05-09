using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APPTemplate.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public PrivacyModel(ILogger<PrivacyModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContext = httpContextAccessor;
        }

        public void OnGet()
        {

            var jeck = _httpContext.HttpContext.Request.Cookies["MyCookie"];
        }
    }
}