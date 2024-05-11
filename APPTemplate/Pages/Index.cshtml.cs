﻿using APPCommon.Class;
using APPTemplate.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace APPTemplate.Pages
{
    public class IndexModel : PageModel
    {

        #region Declaration

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILaptopPassRepository _laptopPassRepository;


        #endregion Declaration

        #region Binding

        #endregion Binding

        #region Constructor

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor, ILaptopPassRepository laptopPassRepository)
        {
            _logger = logger;
            _httpContext = httpContextAccessor;
            _laptopPassRepository = laptopPassRepository;

        }

        #endregion Constructor

        #region Function
        public async Task setRole(string strtitle, string z, string y, string x, string w)
        {
            _httpContext.HttpContext.Session.SetString("MyTitle", strtitle);
            _httpContext.HttpContext.Session.SetString("MyUser", z);
            _httpContext.HttpContext.Session.SetString("MyMode", y);
            _httpContext.HttpContext.Session.SetString("MyBUCode", x);
            _httpContext.HttpContext.Session.SetString("MyRole", w);


            var varlist = await _laptopPassRepository.GetRole();

            var varuserid = varlist.Where(x => x.username == z)
                                   .Select(x => x.userid).FirstOrDefault();
            var varemail = varlist.Where(x => x.username == z)
                                  .Select(x => x.email).FirstOrDefault();
            var varlocalno = varlist.Where(x => x.username == z)
                                 .Select(x => x.localno).FirstOrDefault();

            _httpContext.HttpContext.Session.SetInt32("MyUserId", varuserid);
            _httpContext.HttpContext.Session.SetString("MyEmail", varemail);
            _httpContext.HttpContext.Session.SetString("MyLocalNo", varlocalno);
        }

        #endregion Function

        #region Get

        public async Task<IActionResult> OnGetAsync(string z, string y, string x, string w)
        {
            //Remove  this when publishing
            z = "JERICO VILLANUEVA";
            y = "MENU";
            x = "";
            w = "";


            if (string.IsNullOrEmpty(z))
            {
                return Redirect(PIMESSettings.lnkLogin);
            }
            else
            {
                await setRole("LAPTOP PASS", z, y, x, w);

                return RedirectToPage("Menu");
            }

        }

        #endregion Get


    }
}