using APPCommon.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WEBTemplate.Models;
using WEBTemplate.Repository;
using static WEBTemplate.Models.LaptopPassModel;

namespace WEBTemplate.Pages.Account
{
    public class RoleModel : PageModel
    {
        private readonly ILaptopPassRepository _laptopPassRepository;

        public IEnumerable<LaptopPassRoleModel> roleList { get; set; }

        [BindProperty]
        //public List<LaptopPassRoleModel> searchroleList { get; set; }

        public SelectList buitem { get; set; }
        public SelectList roleitem { get; set; }

        public string username { get; set; }
        public string email { get; set; }
        public string localno { get; set; }
        public string imgstring { get; set; }


        public RoleModel(ILaptopPassRepository laptopPassRepository)
        {
            _laptopPassRepository = laptopPassRepository;
            roleList = new List<LaptopPassRoleModel>();
        }






        public async Task OnGetAsync()
        {
            roleList = (await _laptopPassRepository.GetRole());


            try
            {
                var tempempno = JsonSerializer.Deserialize<List<UserLoginModel>>((string)TempData.Peek("CurrentUser"))
                                              .Select(x => x.employeeno).FirstOrDefault();
                imgstring = "data:image/png;base64," + Convert.ToBase64String(await _laptopPassRepository.GetEmployeeImage(PIMESProcedures.ToInt16OrDefault(tempempno)));



                var tempuser = JsonSerializer.Deserialize<List<UserLoginModel>>((string)TempData.Peek("CurrentUser"))
                                                        .Select(x => x.username).FirstOrDefault();


                // searchroleList = roleList.Where(x => x.username == tempuser).ToList();



                username = roleList.Where(x => x.username == tempuser)
                                   .Select(x => x.username).FirstOrDefault();

                email = roleList.Where(x => x.username == tempuser)
                                  .Select(x => x.email).FirstOrDefault();


                localno = roleList.Where(x => x.username == tempuser)
                                  .Select(x => x.localno).FirstOrDefault();


                buitem = new SelectList(roleList.Where(x => x.username == tempuser).Select(x => x.bucode));

                if (buitem.Count() > 1)
                {



                }
                else
                {


                }







            }
            catch (Exception e)
            {

                var jeck = e.Message;
            }




            //var resultlist = new List<string>();

            //if (!loginList.Any())
            //{
            //    resultlist.Add("");
            //}
            //else
            //{
            //    string myuser = JsonSerializer.Serialize(loginList);

            //    TempData["CurrentUser"] = myuser;

            //    resultlist.Add("loginsucess");
            //}


            //return new JsonResult(resultlist);



            //if (TempData["CurrentUser"] == null)
            //{
            //    return RedirectToPage("/Account/Login");
            //}
            //else
            //{
            //    return Page();
            //}

        }



        public async Task<IActionResult> OnGetCheckRoleAsync(string parabu)
        {
            roleList = (await _laptopPassRepository.GetRole());


            var tempuser = JsonSerializer.Deserialize<List<UserLoginModel>>((string)TempData.Peek("CurrentUser"))
                                                    .Select(x => x.username).FirstOrDefault();

            List<string> varlist = new List<string>();

            var varvar = roleList.Where(x => x.username == tempuser && x.bucode == parabu).ToList();

            foreach (LaptopPassRoleModel item in varvar)
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

    }
}
