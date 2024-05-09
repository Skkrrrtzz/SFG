using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APPTemplate.Pages
{
    public class RequestorModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContext;

        [BindProperty]
        public string pagetitle { get; set; }
        public string webversion { get; set; }

        public RequestorModel(IHttpContextAccessor httpContextAccessor)
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