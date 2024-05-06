using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEBTemplate.Pages
{
    public class NoterModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContext;

        [BindProperty]
        public string pagetitle { get; set; }
        public string webversion { get; set; }

        public NoterModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }



        public void OnGet()
        {
            _httpContext.HttpContext.Session.SetString("MyTitle", "LAPTOP PASS");

            pagetitle = _httpContext.HttpContext.Session.GetString("MyTitle");
            webversion = "Ver. " + APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");
        }
    }
}