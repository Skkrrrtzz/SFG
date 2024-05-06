using APPCommon.Class;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WEBTemplate.Models;
using WEBTemplate.Repository;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static WEBTemplate.Models.LaptopPassModel;

namespace WEBTemplate.Pages.Account
{
    public class RoleModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;


        public IEnumerable<UserRoleModel> roleList { get; set; }

        [BindProperty]
        public List<SelectListItem> buitem2 { get; set; }
        public SelectList buitem { get; set; }
        public SelectList roleitem { get; set; }

        public string username { get; set; }
        public string email { get; set; }
        public string localno { get; set; }
        public string imgstring { get; set; }


        public RoleModel(ILoginRepository loginRepository, IHttpContextAccessor httpContextAccessor)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContextAccessor;
            roleList = new List<UserRoleModel>();

        }






        public async Task OnGetAsync()
        {
            roleList = (await _loginRepository.GetRole());

            if (!roleList.Any())
            {
                return;
            }



            //var tempempno = JsonSerializer.Deserialize<List<UserLoginModel>>(_httpContext.HttpContext.Session.GetString("UserLogin"))
            //                                .Select(x => x.empno).FirstOrDefault();

            imgstring = "data:image/png;base64," + Convert.ToBase64String(await _loginRepository.GetEmployeeImage(PIMESProcedures.ToInt16OrDefault(_httpContext.HttpContext.Session.GetString("MyEmpNo"))));


            var tempuser = _httpContext.HttpContext.Session.GetString("MyUser");


            TempData["UserRole"] = JsonSerializer.Serialize(roleList.Where(x => x.username == tempuser));

            username = roleList.Where(x => x.username == _httpContext.HttpContext.Session.GetString("MyUser"))
                               .Select(x => x.username).FirstOrDefault();

            email = roleList.Where(x => x.username == tempuser)
                            .Select(x => x.email).FirstOrDefault();


            localno = roleList.Where(x => x.username == tempuser)
                              .Select(x => x.localno).FirstOrDefault();



            buitem2 = roleList.Where(x => x.username == tempuser)
                          .Select(x =>
                          new SelectListItem
                          {
                              Value = x.buid.ToString(),
                              Text = x.bucode
                          }).ToList();

            buitem = new SelectList(buitem2, "Value", "Text", null);

            //  var selectedListItem = new SelectListItem();
            //    buitem = new SelectList();

            //    foreach (var item in roleList.Where(x => x.username == tempuser))
            //    {
            //        buitem.Add(new SelectListItem { Text = item.bucode, Value = item.buid.ToString() });



            //    }



            //    buitem = new SelectList(new[]
            //        {



            //});

            //buitem = new SelectList(roleList.Where(x => x.username == tempuser).Select(x => x.bucode), roleList.Where(x => x.username == tempuser).Select(x => x.buid));

            if (buitem.Count() > 1)
            {



            }
            else
            {


            }

        }

        public async Task<IActionResult> OnGetCheckRoleAsync(string parabu)
        {
            roleList = JsonSerializer.Deserialize<List<UserRoleModel>>((string)TempData.Peek("UserRole"));


            List<string> varlist = new List<string>();

            var varvar = roleList.Where(x => x.bucode == parabu).ToList();

            foreach (UserRoleModel item in varvar)
            {
                if (item.requestor > 0)
                {
                    varlist.Add("REQUESTOR");
                }
                if (item.approver > 0)
                {
                    varlist.Add("APPROVER");
                }
                if (item.approver > 0)
                {
                    varlist.Add("NOTER");
                }
            }

            return new JsonResult(varlist);

        }


        public async Task<IActionResult> OnGetStartPageAsync(string parabu, int parabuid, string pararole)
        {
            TempData.Remove("UserRole");

            _httpContext.HttpContext.Session.SetInt32("MyBUId", parabuid);
            _httpContext.HttpContext.Session.SetString("MyBUCode", parabu);
            _httpContext.HttpContext.Session.SetString("MyRole", pararole);

            var result = string.Empty;
            if (pararole == "REQUESTOR")
            {
                result = (JsonSerializer.Serialize(new { currentrole = "REQUESTOR" }));
            }
            else if (pararole=="APPROVER")
            {
                result = (JsonSerializer.Serialize(new { currentrole = "APPROVER" }));
            }
            else
            {
                result = (JsonSerializer.Serialize(new { currentrole = "NOTER" }));
            }

            return new JsonResult(result);
        }

    }
}
