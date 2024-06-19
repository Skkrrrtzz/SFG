using System.Security.Claims;
using APPCommon.Class;
using APPLogin.Models;
using APPLogin.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                // if (string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("MyUser")))
                if (string.IsNullOrEmpty(User.FindFirstValue("FullName")))
                {
                    return RedirectToPage("/Login");
                }
                else
                {
                    // userMenu = await _loginRepository.GetMenu(_httpContext.HttpContext.Session.GetString("MyPassword"));
                    // userPending = await _loginRepository.GetPending(_httpContext.HttpContext.Session.GetString("MyUser"));

                    userMenu = await _loginRepository.GetMenu(User.FindFirstValue("Password"));
                    userPending = await _loginRepository.GetPending(User.FindFirstValue("FullName"));

                    imgstring = "data:image/png;base64," + Convert.ToBase64String(await _loginRepository.GetEmployeeImage(PIMESProcedures.ToInt16OrDefault(User.FindFirstValue("EmpNo"))));
                    user = User.FindFirstValue("FullName").ToUpper();
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

                    // var cookieOptions = new CookieOptions
                    // {
                    //    Path = "/",
                    //    HttpOnly = false,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.None,
                    //    Expires = DateTime.Now.AddSeconds(14)
                    // };

                    // _httpContext.HttpContext.Response.Cookies.Append("MyUser", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);
                    // _httpContext.HttpContext.Response.Cookies.Append("MyPassword", _httpContext.HttpContext.Session.GetString("MyPassword"), cookieOptions);
                    // _httpContext.HttpContext.Response.Cookies.Append("MyLoginMode", "MENU", cookieOptions);
                    // _httpContext.HttpContext.Response.Cookies.Append("MyProgramName", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);
                    // _httpContext.HttpContext.Response.Cookies.Append("MyBUNmame", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);
                    // _httpContext.HttpContext.Response.Cookies.Append("MyRole", _httpContext.HttpContext.Session.GetString("MyUser"), cookieOptions);

                    // var claims = new List<Claim>
                    // {
                    //     new Claim(ClaimTypes.Name, user),
                    //     new Claim(ClaimTypes.Role, "Skibidi"),
                    // };

                    // var claimsIdentity = new ClaimsIdentity(
                    //     claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // await HttpContext.SignInAsync(
                    //     CookieAuthenticationDefaults.AuthenticationScheme, 
                    //     new ClaimsPrincipal(claimsIdentity));

                    Console.WriteLine(User.FindFirstValue("FullName"));

                    return Page();
                }

        }

        #endregion Get
    }
}