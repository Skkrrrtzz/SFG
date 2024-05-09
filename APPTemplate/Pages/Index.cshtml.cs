using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace APPTemplate.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContext;

        [BindProperty]
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }

        public string MyProperty3 { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContext = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync(string user,string pass,string mode)
        {
            MyProperty1 = user;
            MyProperty2 = pass;
            MyProperty3 = mode;

            //var cookieOptions = new CookieOptions
            //{
            //    Path = "/",
            //    HttpOnly = false,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.Now.AddSeconds(14)
            //};

            //MyProperty1 = _httpContext.HttpContext.Request.Cookies["MyUser"];

            //MyProperty2 = _httpContext.HttpContext.Request.Cookies["MyPassword"];

            //MyProperty3 = _httpContext.HttpContext.Request.Cookies["MyLoginMode"];


            //var cookieOptions = new CookieOptions
            //{
            //    Path="/",
            //    HttpOnly = false,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.Now.AddDays(30)
            //};
            // _httpContext.HttpContext.Response.Cookies.Append("MyCookie", "fromjeckcookies", cookieOptions);





            //var jeck = _httpContext.HttpContext.Request.Cookies["MyCookie"];



            //var jeck = _httpContext.HttpContext.Session.GetString("MyUser");

            //if (string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("MyUser")))
            //{
            //    return Redirect("http://192.168.0.188:8081");
            //}
            //else
            //{
            return Page();
            //}

        }
    }
}