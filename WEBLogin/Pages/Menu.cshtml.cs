using APPCommon.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBLogin.Models;
using WEBLogin.Repository;

namespace WEBLogin.Pages
{
    public class MenuModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IEnumerable<UserMenuModel> userLogin { get; set; }

        [BindProperty]
        public string pagetitle { get; set; }

        public string webversion { get; set; } = APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");
        public string imgstring { get; set; }
        public string user { get; set; }
        public string greetings { get; set; }
        public string webtitle { get; set; }
        public string trivia { get; set; }

        public IEnumerable<UserMenuModel> userMenu { get; set; }
        public IEnumerable<UserPendingModel> userPending { get; set; }

        public MenuModel(ILoginRepository loginRepository, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContext;
            _webHostEnvironment = webHostEnvironment;
            userLogin = new List<UserMenuModel>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("MyUser")))
            {
                return RedirectToPage("/Login");
            }
            else
            {
                userMenu = await _loginRepository.GetMenu(_httpContext.HttpContext.Session.GetString("MyPassword"));

                userPending = await _loginRepository.GetPending(_httpContext.HttpContext.Session.GetString("MyUser"));

                _httpContext.HttpContext.Session.SetString("MyTitle", "MENU");

                pagetitle = _httpContext.HttpContext.Session.GetString("MyTitle");

                imgstring = "data:image/png;base64," + Convert.ToBase64String(await _loginRepository.GetEmployeeImage(PIMESProcedures.ToInt16OrDefault(_httpContext.HttpContext.Session.GetString("MyEmpNo"))));
                webtitle = "P I M E S  W E B";

                user = "Hi, " + (_httpContext.HttpContext.Session.GetString("MyUser")).ToUpper();

                greetings = PIMESProcedures.getGreeting();



                using (var sr = new StreamReader(Path.Combine(_webHostEnvironment.WebRootPath, @"txt/trivia.txt")))
                {
                    PIMESVariables.TriviaDB = sr.ReadToEnd().Split('\n');
                }
                trivia = PIMESProcedures.getTrivia(PIMESVariables.TriviaDB);

                return Page();
            }
        }

        public async Task<IActionResult> OnGetUserLoginAsync(string parapass)
        {
            var result = string.Empty;

            //if (!userLogin.Any())
            //{
            //    result = JsonSerializer.Serialize(new { Success = false });
            //}
            //else
            //{
            //    _httpContext.HttpContext.Session.SetString("MyUser", userLogin.Select(x => x.username).FirstOrDefault());
            //    _httpContext.HttpContext.Session.SetString("MyEmpNo", userLogin.Select(x => x.empno).FirstOrDefault());
            //    _httpContext.HttpContext.Session.SetInt32("MyAccess", userLogin.Select(x => x.appaccess).FirstOrDefault());

            //    result = JsonSerializer.Serialize(new { Success = true });
            //}

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetSuccessLoginAsync()
        {
            return RedirectToPage("/Account/Role");
        }
    }
}