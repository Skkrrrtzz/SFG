using APPCommon.Class;
using COMDirectory.Models;
using COMDirectory.Pages.Shared;
using COMDirectory.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Collections;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace COMDirectory.Pages
{
    public class RequestModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDirectoryRepository _directoryRepository;

        public IEnumerable<DirectoryModel> dataDirectory { get; set; }

        public List<DirectoryModel> directory { get; set; } 

        [BindProperty]
        public string pagetitle { get; set; }
        public string webversion { get; set; }

        public RequestModel(IHttpContextAccessor httpContextAccessor, IDirectoryRepository directoryRepository)
        {
            _httpContext = httpContextAccessor;
            _directoryRepository = directoryRepository;
            dataDirectory = new List<DirectoryModel>();
        }



        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Version"] = "Ver. " + APPCommon.RevisionHistory.RevisionHistory.appVersion;



            _httpContext.HttpContext.Session.SetString("MyTitle", "DIRECTORY");

            pagetitle = _httpContext.HttpContext.Session.GetString("MyTitle");
            webversion = "Ver. " + APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");


            try
            {
                dataDirectory = await _directoryRepository.GetDirectory();

                var result = string.Empty;

                if (!dataDirectory.Any())
                {
                    result = JsonSerializer.Serialize(new { Success = false });
                }
                else
                {
                    result = JsonSerializer.Serialize(new { Success = true });
                }

                return Page();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = ex.Message });
            }
        }





        public async Task<IActionResult> OnGetDetailsAsync(string data)
        {
            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
            };

            var result = JsonSerializer.Deserialize<DirectoryModel>(data, options);

            if (result.empcode == 0)
            {
                TempData["imgEmployee"] = null;
            }
            else
            {
                TempData["imgEmployee"] = "data:image/png;base64," + Convert.ToBase64String(await _directoryRepository.GetEmployeeImage(result.empcode));

            }

            return Partial("_DirectoryDetails", result);
        }

    }
}