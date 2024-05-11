using APPCommon.Class;
using APPLogin.Models;
using APPLogin.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APPLogin.Pages
{
    public class MenuModel : PageModel
    {
        #region Declaration

        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion Declaration

        #region Binding

        [BindProperty]
        public string user { get; set; }

        public string mode { get; set; }
        public string bucode { get; set; }
        public string role { get; set; }
        public string imgstring { get; set; }
        public string greetings { get; set; }
        public string trivia { get; set; }
        public string webversion { get; set; } = APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");

        public IEnumerable<UserMenuModel> userMenu { get; set; }
        public IEnumerable<UserPendingModel> userPending { get; set; }
        public IEnumerable<UserMenuModel> userLogin { get; set; }

        #endregion Binding

        #region Constructor

        public MenuModel(ILoginRepository loginRepository, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContext;
            _webHostEnvironment = webHostEnvironment;
            userLogin = new List<UserMenuModel>();
        }

        #endregion Constructor

        #region Get

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("MyUser")))
                {
                    return RedirectToPage("/Login");
                }
                else
                {
                    userMenu = await _loginRepository.GetMenu(_httpContext.HttpContext.Session.GetString("MyPassword"));
                    userPending = await _loginRepository.GetPending(_httpContext.HttpContext.Session.GetString("MyUser"));

                    imgstring = "data:image/png;base64," + Convert.ToBase64String(await _loginRepository.GetEmployeeImage(PIMESProcedures.ToInt16OrDefault(_httpContext.HttpContext.Session.GetString("MyEmpNo"))));
                    user = (_httpContext.HttpContext.Session.GetString("MyUser")).ToUpper();
                    greetings = PIMESProcedures.getGreeting();

                    using (var sr = new StreamReader(Path.Combine(_webHostEnvironment.WebRootPath, @"txt/trivia.txt")))
                    {
                        PIMESVariables.TriviaDB = sr.ReadToEnd().Split('\n');
                    }
                    trivia = PIMESProcedures.getTrivia(PIMESVariables.TriviaDB);

                    mode = "MENU";
                    bucode = "";
                    role = "";

                    //Do not remove Cookie Implementation

                    //var cookieOptions = new CookieOptions
                    //{
                    //    Path = "/",
                    //    HttpOnly = false,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.None,
                    //    Expires = DateTime.Now.AddSeconds(14)
                    //};

                    //_httpContext.HttpContext.Response.Cookies.Append("MyUser", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);
                    //_httpContext.HttpContext.Response.Cookies.Append("MyPassword", _httpContext.HttpContext.Session.GetString("MyPassword"), cookieOptions);
                    //_httpContext.HttpContext.Response.Cookies.Append("MyLoginMode", "MENU", cookieOptions);
                    //_httpContext.HttpContext.Response.Cookies.Append("MyProgramName", userMenu., cookieOptions);
                    //_httpContext.HttpContext.Response.Cookies.Append("MyBUNmame", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);
                    //_httpContext.HttpContext.Response.Cookies.Append("MyRole", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);

                    return Page();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = ex.Message });
            }
        }

        #endregion Get
    }
}