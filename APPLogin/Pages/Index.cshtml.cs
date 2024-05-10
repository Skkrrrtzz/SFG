using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APPLogin.Pages
{
    public class IndexModel : PageModel
    {
        #region Declaration

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContext;

        #endregion Declaration



        #region Constructor

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContext = httpContextAccessor;
        }

        #endregion Constructor

        #region Get

        public async Task<IActionResult> OnGetAsync()
        {
            _httpContext.HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }

        #endregion Get
    }
}