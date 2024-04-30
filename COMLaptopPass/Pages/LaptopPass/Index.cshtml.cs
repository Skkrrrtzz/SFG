using COMLaptopPass.Models;
using COMLaptopPass.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Data;

namespace COMLaptopPass.Pages.LaptopPass
{
    public class IndexModel : PageModel
    {
        private readonly ILaptopPassRepository laptopPassRepository;

        public IEnumerable<LaptopPassRequestModel> requestList { get; set; } = new List<LaptopPassRequestModel>();

        [BindProperty]
        public LaptopPassParameters param { get; set; } = new LaptopPassParameters();



        public IndexModel(ILaptopPassRepository _laptopPassRepository)
        {
            param.mode = "read";
            param.role = "REQUESTOR";
            param.userid = 2;
            param.startdate = DateTime.Today.AddMonths(-6);
            param.stopdate = DateTime.Today;

            laptopPassRepository = _laptopPassRepository;
        }
        public async Task OnGetAsync()
        {

            requestList = (await laptopPassRepository.GetRequest(param)).ToList();

        }



        public async Task OnPostJeckAsync()
        {
            param.mode = "read";
            param.role = "REQUESTOR";
            param.userid = 2;

            requestList = (await laptopPassRepository.GetRequest(param)).ToList();

        }
    }
}
